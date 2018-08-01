﻿using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Formatting.Tokenizer;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tests.Tokenizer
{
    [TestFixture]
    internal class AllNamedTokensFactory_Tests
    {
        private AllNamedTokensFactory factory;

        [SetUp]
        public void TestSetup()
        {
            factory = new AllNamedTokensFactory();
        }

        [Test]
        public void Should_create_exception_token_with_given_format()
        {
            factory.Create(PropertyNames.Exception, "format").Should().BeOfType<ExceptionToken>()
                .Which.Format.Should().Be("format");
        }

        [Test]
        public void Should_create_level_token_with_given_format()
        {
            factory.Create(PropertyNames.Level, "format").Should().BeOfType<LevelToken>()
                .Which.Format.Should().Be("format");
        }

        [Test]
        public void Should_create_message_token_with_given_format()
        {
            factory.Create(PropertyNames.Message, "format").Should().BeOfType<MessageToken>()
                .Which.Format.Should().Be("format");
        }

        [Test]
        public void Should_create_newline_token_with_given_format()
        {
            factory.Create(PropertyNames.NewLine, "format").Should().BeOfType<NewlineToken>()
                .Which.Format.Should().Be("format");
        }

        [Test]
        public void Should_create_properties_token_with_given_format()
        {
            factory.Create(PropertyNames.Properties, "format").Should().BeOfType<PropertiesToken>()
                .Which.Format.Should().Be("format");
        }

        [Test]
        public void Should_create_timestamp_token_with_given_format()
        {
            factory.Create(PropertyNames.Timestamp, "format").Should().BeOfType<TimestampToken>()
                .Which.Format.Should().Be("format");
        }

        [Test]
        public void Should_create_uptime_token_with_given_format()
        {
            factory.Create(PropertyNames.Uptime, "format").Should().BeOfType<UptimeToken>()
                .Which.Format.Should().Be("format");
        }

        [Test]
        public void Should_create_property_tokens_for_non_special_names()
        {
            factory.Create("name", "format").Should().BeEquivalentTo(new PropertyToken("name", "format"));
        }

        [Test]
        public void Should_be_case_insensitive_for_special_token_names()
        {
            factory.Create(PropertyNames.Timestamp.ToLowerInvariant(), "format").Should().BeOfType<TimestampToken>()
                .Which.Format.Should().Be("format");

            factory.Create(PropertyNames.Message.ToUpperInvariant(), "format").Should().BeOfType<MessageToken>()
                .Which.Format.Should().Be("format");
        }
    }
}