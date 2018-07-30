using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Formatting.Tokenizer;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tests.Tokenizer
{
    [TestFixture]
    internal class SpecialTokenFactory_Tests
    {
        private SpecialTokenFactory factory;

        [SetUp]
        public void TestSetup()
        {
            factory = new SpecialTokenFactory();
        }

        [Test]
        public void Should_create_exception_token_with_given_format()
        {
            factory.Create("Exception", "format").Should().BeOfType<ExceptionToken>()
                .Which.Format.Should().Be("format");
        }

        [Test]
        public void Should_create_level_token_with_given_format()
        {
            factory.Create("Level", "format").Should().BeOfType<LevelToken>()
                .Which.Format.Should().Be("format");
        }

        [Test]
        public void Should_create_message_token_with_given_format()
        {
            factory.Create("Message", "format").Should().BeOfType<MessageToken>()
                .Which.Format.Should().Be("format");
        }

        [Test]
        public void Should_newline_message_token_with_given_format()
        {
            factory.Create("NewLine", "format").Should().BeOfType<NewlineToken>()
                .Which.Format.Should().Be("format");
        }

        [Test]
        public void Should_newline_properties_token_with_given_format()
        {
            factory.Create("Properties", "format").Should().BeOfType<PropertiesToken>()
                .Which.Format.Should().Be("format");
        }

        [Test]
        public void Should_newline_timestamp_token_with_given_format()
        {
            factory.Create("Timestamp", "format").Should().BeOfType<TimestampToken>()
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
            factory.Create("timestamp", "format").Should().BeOfType<TimestampToken>()
                .Which.Format.Should().Be("format");

            factory.Create("MESSAGE", "format").Should().BeOfType<MessageToken>()
                .Which.Format.Should().Be("format");
        }
    }
}