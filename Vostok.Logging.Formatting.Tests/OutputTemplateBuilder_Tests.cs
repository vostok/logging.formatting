﻿using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tests
{
    [TestFixture]
    internal class OutputTemplateBuilder_Tests
    {
        private OutputTemplateBuilder builder;

        [SetUp]
        public void TestSetup()
        {
            builder = new OutputTemplateBuilder();
        }

        [Test]
        public void AddText_should_add_text_token_with_given_text()
        {
            builder.AddText("123");

            builder.Build().Tokens.Should().ContainSingle().Which.Should().BeOfType<TextToken>().Which.Text.Should().Be("123");
        }

        [Test]
        public void AddNewline_should_add_a_newline_token()
        {
            builder.AddNewline();

            builder.Build().Tokens.Should().ContainSingle().Which.Should().BeOfType<NewlineToken>();
        }

        [Test]
        public void AddTimestamp_should_add_a_timestamp_token_with_given_format()
        {
            builder.AddTimestamp("YYYY");

            builder.Build().Tokens.Should().ContainSingle().Which.Should().BeOfType<TimestampToken>().Which.Format.Should().Be("YYYY");
        }

        [Test]
        public void AddUptime_should_add_an_uptime_token_with_given_format()
        {
            builder.AddUptime("D10");

            builder.Build().Tokens.Should().ContainSingle().Which.Should().BeOfType<UptimeToken>().Which.Format.Should().Be("D10");
        }

        [Test]
        public void AddProperty_should_add_a_property_token_with_given_named_and_format()
        {
            builder.AddProperty("prop", "format");

            var token = builder.Build().Tokens.Should().ContainSingle().Which.Should().BeOfType<PropertyToken>().Which;

            token.Name.Should().Be("prop");
            token.Format.Should().Be("format");
        }

        [Test]
        public void AddAllProperties_should_add_a_properties_token()
        {
            builder.AddAllProperties();

            builder.Build().Tokens.Should().ContainSingle().Which.Should().BeOfType<PropertiesToken>();
        }

        [Test]
        public void AddMessage_should_add_a_message_token()
        {
            builder.AddMessage();

            builder.Build().Tokens.Should().ContainSingle().Which.Should().BeOfType<MessageToken>();
        }

        [Test]
        public void AddLevel_should_add_a_level_token()
        {
            builder.AddLevel();

            builder.Build().Tokens.Should().ContainSingle().Which.Should().BeOfType<LevelToken>();
        }

        [Test]
        public void AddException_should_add_an_exception_token()
        {
            builder.AddException();

            builder.Build().Tokens.Should().ContainSingle().Which.Should().BeOfType<ExceptionToken>();
        }
        
        [Test]
        public void AddProperty_OperationContext_should_add_an_operation_context_token()
        {
            builder.AddProperty(WellKnownProperties.OperationContext);

            builder.Build().Tokens.Should().ContainSingle().Which.Should().BeOfType<OperationContextToken>();
        }

        [Test]
        public void Should_be_able_to_assemble_template_from_multiple_tokens()
        {
            builder
                .AddText("1")
                .AddText("2")
                .AddText("3")
                .AddProperty("prop")
                .Build()
                .Tokens.Should()
                .HaveCount(4);
        }
    }
}