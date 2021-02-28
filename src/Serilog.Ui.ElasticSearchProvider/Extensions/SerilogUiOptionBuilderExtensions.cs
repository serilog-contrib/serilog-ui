using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog.Ui.Core;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.Ui.ElasticSearchProvider.Extensions
{
    public static class SerilogUiOptionBuilderExtensions
    {
        public static void UseElasticSearchDb(this SerilogUiOptionsBuilder optionsBuilder,
                                      Uri endpoint,
                                      string indexName)
        {
            if (endpoint == null) throw new ArgumentNullException(nameof(endpoint));
            if (string.IsNullOrEmpty(indexName)) throw new ArgumentNullException(nameof(indexName));

            var options = new ElasticSearchDbOptions
            {
                IndexName = indexName
            };

            var builder = ((ISerilogUiOptionsBuilder)optionsBuilder);

            builder.Services.AddSingleton(options);

            var pool = new SingleNodeConnectionPool(endpoint);
            //var connectionSettings1 = new ConnectionSettings(pool, sourceSerializer: JsonNetSerializer.Default);
            //var connectionSettings = new ConnectionSettings(pool, sourceSerializer: (builtin, values) => new CamelCaseJsonNetSerializer(builtin, values));
            var connectionSettings = new ConnectionSettings(pool, sourceSerializer: (builtin, values) => new VanillaSerializer());

            //var connectionSettings = new ConnectionSettings(pool, (builtin, settings) =>
            //    new JsonNetSerializer(builtin, settings,
            //        modifyContractResolver: c => { c.NamingStrategy = new SnakeCaseNamingStrategy(); }
            //    )
            //);


            builder.Services.AddSingleton<IElasticClient>(o => new ElasticClient(connectionSettings));
            builder.Services.AddScoped<IDataProvider, ElasticSearchDbDataProvider>();
        }
    }

    class CamelCaseJsonNetSerializer : ConnectionSettingsAwareSerializerBase
    {
        public CamelCaseJsonNetSerializer(IElasticsearchSerializer builtinSerializer, IConnectionSettingsValues connectionSettings)
            : base(builtinSerializer, connectionSettings) { }

        protected override void ModifyContractResolver(ConnectionSettingsAwareContractResolver resolver) =>
            resolver.NamingStrategy = new CamelCaseNamingStrategy();

        public override Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            using (StreamReader reader = new StreamReader(stream))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                JsonSerializer ser = new JsonSerializer();
                return Task.FromResult(ser.Deserialize<T>(jsonReader));
            }
        }

        public override Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default)
        {
            using (StreamReader reader = new StreamReader(stream))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                JsonSerializer ser = new JsonSerializer();
                return Task.FromResult(ser.Deserialize(jsonReader, type));
            }
        }
    }

    public class VanillaSerializer : IElasticsearchSerializer
    {
        public T Deserialize<T>(Stream stream)
         => (T)Deserialize(typeof(T), stream);


        public object Deserialize(Type type, Stream stream)
        {
#if DEBUG
            var memStream = new MemoryStream();
            stream.CopyTo(memStream);
            memStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(memStream);
            var @string = reader.ReadToEnd();

            memStream.Seek(0, SeekOrigin.Begin);

            using (var jreader = new JsonTextReader(reader))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize(jreader, type);
            }
#else
            var reader = new StreamReader(stream);
            using (var jreader = new JsonTextReader(reader))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize(jreader, type);
            }
#endif
        }

        public Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default(CancellationToken)) =>
            Task.FromResult(Deserialize<T>(stream));

        public Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default(CancellationToken)) =>
            Task.FromResult(Deserialize(type, stream));

        public void Serialize<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.Indented)
        {
            var writer = new StreamWriter(stream);
            using (var jWriter = new JsonTextWriter(writer))
            {
                var serializer = new JsonSerializer
                {
                    Formatting = formatting == SerializationFormatting.Indented ? Formatting.Indented : Formatting.None
                };
                serializer.Serialize(jWriter, data);
            }
        }


        public Task SerializeAsync<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.Indented,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Serialize<T>(data, stream, formatting);
            return Task.CompletedTask;
        }
    }
}
