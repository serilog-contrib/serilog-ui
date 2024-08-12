using System.IO;
using System.Text;
using System.Threading.Tasks;
using Elastic.Elasticsearch.Xunit.XunitPlumbing;
using FluentAssertions;
using Newtonsoft.Json;
using Serilog.Ui.ElasticSearchProvider.Serializers;
using Xunit;
using MemoryStream = System.IO.MemoryStream;

namespace ElasticSearch.Tests.Serializers;

[Trait("Unit-Serializers", "Elastic")]
public class VanillaSerializerTest
{
    private readonly VanillaSerializer _sut = new();

    private static readonly TestClass _sample = new() { MyProperty = 10, MyProperty2 = "string" };

    private static readonly string _sampleSerialized = JsonConvert.SerializeObject(_sample);
    
    [U]
    public void It_deserializes_property()
    {
        // Arrange
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(_sampleSerialized));
        using var streamWithType = new MemoryStream(Encoding.UTF8.GetBytes(_sampleSerialized));

        // Act
        var act = _sut.Deserialize<TestClass>(stream);
        var actWithType = _sut.Deserialize(_sample.GetType(), streamWithType);

        // Assert
        act.Should().BeEquivalentTo(_sample);
        actWithType.Should().BeEquivalentTo(_sample);
    }

    [U]
    public async Task It_deserializes_property_async()
    {
        // Arrange
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(_sampleSerialized));
        using var streamWithType = new MemoryStream(Encoding.UTF8.GetBytes(_sampleSerialized));

        // Act
        var act = await _sut.DeserializeAsync<TestClass>(stream);
        var actWithType = await _sut.DeserializeAsync(_sample.GetType(), streamWithType);

        // Assert
        act.Should().BeEquivalentTo(_sample);
        actWithType.Should().BeEquivalentTo(_sample);
    }
    
    [U]
    public async Task It_serializes_property()
    {
        // Arrange
        using var stream = new MemoryStream();

        // Act
        _sut.Serialize(_sample.GetType(), stream);

        // Assert
        using var streamRead = new MemoryStream(stream.ToArray());
        using var reader = new StreamReader(streamRead);

        var result = await reader.ReadToEndAsync();
        result.Should().NotBeEmpty();
    }

    [U]
    public async Task It_serializes_property_async()
    {
        // Arrange
        using var stream = new MemoryStream();

        // Act
        await _sut.SerializeAsync(_sample.GetType(), stream);

        // Assert
        using var streamRead = new MemoryStream(stream.ToArray());
        using var reader = new StreamReader(streamRead);

        var result = await reader.ReadToEndAsync();
        result.Should().NotBeEmpty();
    }
    
    private class TestClass
    {
        public int MyProperty { get; set; }

        public string MyProperty2 { get; set; } = string.Empty;
    }
}