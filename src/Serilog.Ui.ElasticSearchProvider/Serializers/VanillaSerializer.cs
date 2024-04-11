using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Newtonsoft.Json;

namespace Serilog.Ui.ElasticSearchProvider.Serializers
{
    /// <summary>
    /// Note: don't remove <see cref="Newtonsoft.Json" /> as the provider will break on Exception serialization.
    /// </summary>
    internal class VanillaSerializer : IElasticsearchSerializer
    {
        public T Deserialize<T>(Stream stream) => (T)Deserialize(typeof(T), stream);

        public object Deserialize(Type type, Stream stream)
        {
            var reader = new StreamReader(stream);

            using (var jsonTextReader = new JsonTextReader(reader))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize(jsonTextReader, type);
            }
        }

        public Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default) =>
            Task.FromResult(Deserialize<T>(stream));

        public Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default) =>
            Task.FromResult(Deserialize(type, stream));

        public void Serialize<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.None)
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

        public Task SerializeAsync<T>(
            T data,
            Stream stream,
            SerializationFormatting formatting = SerializationFormatting.None,
            CancellationToken cancellationToken = default)
        {
            Serialize(data, stream, formatting);

            return Task.CompletedTask;
        }
    }
}