using FluentAssertions;
using Serilog.Ui.Web;
using System;
using Xunit;

namespace Ui.Web.Tests.Extensions
{
    [Trait("Ui-Injection", "Web")]
    public class ContentInjectionTest
    {
        [Fact]
        public void It_appends_script_tag_with_defaults()
        {
            // Act
            var ui = new UiOptions().InjectJavascript("test/path");

            // Assert
            ui.HeadContent.Should().BeEmpty();
            ui.BodyContent.Trim().Should().Be("<script src='test/path' type='text/javascript'></script>");
        }

        [Fact]
        public void It_appends_script_tag_with_customs()
        {
            // Act
            var ui = new UiOptions().InjectJavascript("test/path", true, "text/text");

            // Assert
            ui.BodyContent.Should().BeEmpty();
            ui.HeadContent.Trim().Should().Be("<script src='test/path' type='text/text'></script>");
        }

        [Fact]
        public void It_appends_multiple_script_tags()
        {
            // Act
            var ui = new UiOptions().InjectJavascript("test/path").InjectJavascript("test/path");

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
            var ui = new UiOptions().InjectStylesheet("test/path");

            // Assert
            ui.HeadContent.Trim().Should().Be("<link href='test/path' rel='stylesheet' media='screen' type='text/css' />");
            ui.BodyContent.Should().BeEmpty();
        }

        [Fact]
        public void It_appends_stylesheet_tag_with_customs()
        {
            // Act
            var ui = new UiOptions().InjectStylesheet("test/path", "test");

            // Assert
            ui.HeadContent.Trim().Should().Be("<link href='test/path' rel='stylesheet' media='test' type='text/css' />");
            ui.BodyContent.Should().BeEmpty();
        }

        [Fact]
        public void It_appends_multiple_stylesheet_tags()
        {
            // Act
            var ui = new UiOptions().InjectStylesheet("test/path").InjectStylesheet("test/path");

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
            var ui = new UiOptions()
                .InjectJavascript("test/path")
                .InjectStylesheet("test/path")
                .InjectJavascript("test/path", true, "text/test");

            // Assert
            ui.HeadContent.Trim().Should().Be(
                $"<link href='test/path' rel='stylesheet' media='screen' type='text/css' />{Environment.NewLine}" +
                "<script src='test/path' type='text/test'></script>");
            ui.BodyContent.Trim().Should().Be("<script src='test/path' type='text/javascript'></script>");
        }
    }
}
