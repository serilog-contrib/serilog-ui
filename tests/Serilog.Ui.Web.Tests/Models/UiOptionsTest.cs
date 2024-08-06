using System;
using FluentAssertions;
using Serilog.Ui.Core.Models.Options;
using Serilog.Ui.Web.Models;
using Xunit;

namespace Serilog.Ui.Web.Tests.Models;

[Trait("Ui-UiOptions", "Web")]
public class UiOptionsTest
{
    [Fact]
    public void It_gets_default_options()
    {
        var sut = GetOptions();

        sut.HomeUrl.Should().Be("/");
        sut.RoutePrefix.Should().Be("serilog-ui");
        sut.BodyContent.Should().BeEmpty();
        sut.HeadContent.Should().BeEmpty();
        sut.ShowBrand.Should().BeTrue();
        sut.Authorization.AuthenticationType.Should().Be(AuthenticationType.Custom);
        sut.Authorization.RunAuthorizationFilterOnAppRoutes.Should().BeFalse();
    }

    [Fact]
    public void It_sets_options()
    {
        var sut = GetOptions();

        sut
            .WithHomeUrl("test")
            .WithAuthenticationType(AuthenticationType.Basic)
            .WithRoutePrefix("prefix")
            .HideSerilogUiBrand()
            .EnableAuthorizationOnAppRoutes();

        sut.HomeUrl.Should().Be("test");
        sut.RoutePrefix.Should().Be("prefix");
        sut.ShowBrand.Should().BeFalse();
        sut.Authorization.AuthenticationType.Should().Be(AuthenticationType.Basic);
        sut.Authorization.RunAuthorizationFilterOnAppRoutes.Should().BeTrue();
    }

    [Fact]
    public void It_appends_script_tag_with_defaults()
    {
        // Act
        var ui = GetOptions().InjectJavascript("test/path");

        // Assert
        ui.HeadContent.Should().BeEmpty();
        ui.BodyContent.Trim().Should().Be("<script src='test/path' type='text/javascript'></script>");
    }

    [Fact]
    public void It_appends_script_tag_with_customs()
    {
        // Act
        var ui = GetOptions().InjectJavascript("test/path", true, "text/text");

        // Assert
        ui.BodyContent.Should().BeEmpty();
        ui.HeadContent.Trim().Should().Be("<script src='test/path' type='text/text'></script>");
    }

    [Fact]
    public void It_appends_multiple_script_tags()
    {
        // Act
        var ui = GetOptions().InjectJavascript("test/path").InjectJavascript("test/path");

        // Assert
        ui.HeadContent.Should().BeEmpty();
        ui.BodyContent.Trim().Should().Be(
            $"<script src='test/path' type='text/javascript'></script>{Environment.NewLine}" +
            "<script src='test/path' type='text/javascript'></script>");
    }

    [Fact]
    public void It_appends_stylesheet_tag_with_defaults()
    {
        // Act
        var ui = GetOptions().InjectStylesheet("test/path");

        // Assert
        ui.HeadContent.Trim().Should().Be("<link href='test/path' rel='stylesheet' media='screen' type='text/css' />");
        ui.BodyContent.Should().BeEmpty();
    }

    [Fact]
    public void It_appends_stylesheet_tag_with_customs()
    {
        // Act
        var ui = GetOptions().InjectStylesheet("test/path", "test");

        // Assert
        ui.HeadContent.Trim().Should().Be("<link href='test/path' rel='stylesheet' media='test' type='text/css' />");
        ui.BodyContent.Should().BeEmpty();
    }

    [Fact]
    public void It_appends_multiple_stylesheet_tags()
    {
        // Act
        var ui = GetOptions().InjectStylesheet("test/path").InjectStylesheet("test/path");

        // Assert
        ui.HeadContent.Trim().Should().Be(
            $"<link href='test/path' rel='stylesheet' media='screen' type='text/css' />{Environment.NewLine}" +
            "<link href='test/path' rel='stylesheet' media='screen' type='text/css' />");
        ui.BodyContent.Should().BeEmpty();
    }

    [Fact]
    public void It_mix_scripts_and_stylesheets()
    {
        // Act
        var ui = GetOptions()
            .InjectJavascript("test/path")
            .InjectStylesheet("test/path")
            .InjectJavascript("test/path", true, "text/test");

        // Assert
        ui.HeadContent.Trim().Should().Be(
            $"<link href='test/path' rel='stylesheet' media='screen' type='text/css' />{Environment.NewLine}" +
            "<script src='test/path' type='text/test'></script>");
        ui.BodyContent.Trim().Should().Be("<script src='test/path' type='text/javascript'></script>");
    }

    [Fact]
    public void It_validates_options()
    {
        var error = () => GetOptions().WithRoutePrefix("my-prefix/").Validate();

        error.Should().ThrowExactly<ArgumentException>().WithMessage("RoutePrefix can't end with a slash.");
    }

    private static UiOptions GetOptions() => new(new ProvidersOptions());
}