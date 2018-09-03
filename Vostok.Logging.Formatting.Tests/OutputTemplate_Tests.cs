using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tests
{
    [TestFixture]
    internal class OutputTemplate_Tests
    {
        [Test]
        public void Should_always_return_same_result_from_tostring()
        {
            var result1 = OutputTemplate.Default.ToString();
            var result2 = OutputTemplate.Default.ToString();

            result2.Should().BeSameAs(result1);
        }

        [Test]
        public void Should_successfully_parse_from_string_with_special_properties()
        {
            var template = OutputTemplate.Parse("lalala{Level}{Message}lalala");

            template.Tokens.Should().HaveCount(4);
            template.Tokens[0].Should().BeOfType<TextToken>().Which.Text.Should().Be("lalala");
            template.Tokens[1].Should().BeOfType<LevelToken>();
            template.Tokens[2].Should().BeOfType<MessageToken>();
            template.Tokens[3].Should().BeOfType<TextToken>().Which.Text.Should().Be("lalala");
        }

        [Test]
        public void Two_templates_with_equivalent_tokens_should_be_considered_equal()
        {
            var template1 = OutputTemplate.Parse("lalala{Level}{Message}lalala");
            var template2 = OutputTemplate.Parse("lalala{Level}{Message}lalala");

            template2.Should().Be(template1);
        }

        [Test]
        public void Two_templates_with_different_tokens_should_not_be_considered_equal()
        {
            var template1 = OutputTemplate.Parse("lalala{Prop1}{Message}lalala");
            var template2 = OutputTemplate.Parse("lalala{Prop2}{Message}lalala");

            template2.Should().NotBe(template1);
        }

        [Test]
        public void Two_templates_with_similar_tokens_but_different_content_should_not_be_considered_equal()
        {
            var template1 = OutputTemplate.Parse("lalala{Prop:f1}{Message}lalala");
            var template2 = OutputTemplate.Parse("lalala{Prop:f2}{Message}lalala");

            template2.Should().NotBe(template1);
        }
    }
}