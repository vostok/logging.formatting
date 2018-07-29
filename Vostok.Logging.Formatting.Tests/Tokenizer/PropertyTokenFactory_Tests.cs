using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Formatting.Tokenizer;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tests.Tokenizer
{
    [TestFixture]
    internal class PropertyTokenFactory_Tests
    {
        private PropertyTokenFactory factory;

        [SetUp]
        public void TestSetup()
        {
            factory = new PropertyTokenFactory();
        }

        [Test]
        public void Should_return_a_property_token_with_given_name_and_format()
        {
            var token = factory.Create("name", "format").Should().BeOfType<PropertyToken>().Which;

            token.Name.Should().Be("name");
            token.Format.Should().Be("format");
        }
    }
}