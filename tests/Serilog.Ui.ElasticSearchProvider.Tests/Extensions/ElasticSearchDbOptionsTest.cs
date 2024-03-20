using System;
using System.Collections.Generic;
using Elastic.Elasticsearch.Xunit.XunitPlumbing;
using ElasticSearch.Tests.Util;
using FluentAssertions;
using Serilog.Ui.ElasticSearchProvider;
using Xunit;

namespace ElasticSearch.Tests.Extensions;

[Trait("Unit-ElasticSearchDbOptions", "Elastic")]
public class ElasticSearchDbOptionsTest : IClusterFixture<Elasticsearch7XCluster>
{
    [U]
    public void It_validates_options()
    {
        var uri = new Uri("https://elastic.example.com");
        var result = () => new ElasticSearchDbOptions().WithEndpoint(uri).WithIndex("test").Validate();
        result.Should().NotThrow();
    }

    [U]
    public void It_throws_on_validation_failed()
    {
        var uri = new Uri("https://elastic.example.com");
        var nullables = new List<Action>
        {
            () => new ElasticSearchDbOptions().WithIndex("name").Validate(),
            () => new ElasticSearchDbOptions().WithEndpoint(uri).Validate(),
            () => new ElasticSearchDbOptions().WithEndpoint(uri).WithIndex(" ").Validate(),
            () => new ElasticSearchDbOptions().WithEndpoint(uri).WithIndex("").Validate(),
        };

        foreach (var nullable in nullables)
        {
            nullable.Should().Throw<ArgumentException>();
        }
    }
}