﻿using FluentAssertions;
using Serilog.Ui.Web;
using Xunit;

namespace Ui.Web.Tests.Extensions
{
    [Trait("Ui-Injection", "Web")]
    public class ContentInjectionTest
    {
        [Fact]
        public void It_appends_script_tag_with_defaults()
        {
            var ui = new UiOptions().InjectJavascript("test/path");

            ui.HeadContent.Should().BeEmpty();
            ui.BodyContent.Trim().Should().Be("<script src='test/path' type='text/javascript'></script>");
        }

        [Fact]
        public void It_appends_script_tag_with_customs()
        {
            var ui = new UiOptions().InjectJavascript("test/path", true, "text/text");

            ui.BodyContent.Should().BeEmpty();
            ui.HeadContent.Trim().Should().Be("<script src='test/path' type='text/text'></script>");
        }

        [Fact]
        public void It_appends_multiple_script_tags()
        {
            var ui = new UiOptions().InjectJavascript("test/path").InjectJavascript("test/path");

            ui.HeadContent.Should().BeEmpty();
            ui.BodyContent.Trim().Should().Be(
                "<script src='test/path' type='text/javascript'></script>" +
                "<script src='test/path' type='text/javascript'></script>");
        }

        [Fact]
        public void It_appends_stylesheet_tag_with_defaults()
        {
            var ui = new UiOptions().InjectStylesheet("test/path");

            ui.HeadContent.Trim().Should().Be("<link href='test/path' rel='stylesheet' media='screen' type='text/css' />");
            ui.BodyContent.Should().BeEmpty();
        }

        [Fact]
        public void It_appends_stylesheet_tag_with_customs()
        {
            var ui = new UiOptions().InjectStylesheet("test/path", "test");

            ui.HeadContent.Trim().Should().Be("<link href='test/path' rel='stylesheet' media='test' type='text/css' />");
            ui.BodyContent.Should().BeEmpty();
        }

        [Fact]
        public void It_appends_multiple_stylesheet_tags()
        {
            var ui = new UiOptions().InjectStylesheet("test/path").InjectStylesheet("test/path");

            ui.HeadContent.Trim().Should().Be(
                "<link href='test/path' rel='stylesheet' media='screen' type='text/css' />" +
                "<link href='test/path' rel='stylesheet' media='screen' type='text/css' />");
            ui.BodyContent.Should().BeEmpty();
        }

        [Fact]
        public void It_mix_scripts_and_stylesheets()
        {
            var ui = new UiOptions()
                .InjectJavascript("test/path")
                .InjectStylesheet("test/path")
                .InjectJavascript("test/path", true, "text/test");

            ui.HeadContent.Trim().Should().Be(
                "<link href='test/path' rel='stylesheet' media='screen' type='text/css' />" +
                "<script src='test/path' type='text/test'></script>");
            ui.BodyContent.Trim().Should().Be("<script src='test/path' type='text/javascript'></script>");
        }
    }
}