using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Formatting.Tokenizer;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tests.Tokenizer
{
    [TestFixture]
    internal class TemplateTokenizer_Tests
    {
        [Test]
        public void Should_produce_no_tokens_for_null_template()
        {
            Test(null);
        }

        [Test]
        public void Should_produce_no_tokens_for_empty_template()
        {
            Test(string.Empty);
        }

        [Test]
        public void Should_produce_single_text_token_for_whitespace_template()
        {
            Test("   ", new TextToken("   "));
        }

        [TestCase("Hello, world!")]
        [TestCase("!@#$%^&*()")]
        public void Should_produce_single_text_token_for_template_without_properties(string template)
        {
            Test(template, new TextToken(template));
        }

        [TestCase("Hello")]
        [TestCase("Hello123")]
        [TestCase("0")]
        [TestCase("1")]
        [TestCase("_underscored")]
        public void Should_produce_single_property_token_for_template_with_a_single_property_without_format(string name)
        {
            Test($"{{{name}}}", new PropertyToken(name));
        }

        [TestCase("Hello")]
        [TestCase("Hello123")]
        [TestCase("0")]
        [TestCase("1")]
        [TestCase("_underscored")]
        public void Should_produce_single_property_token_for_template_with_a_single_property_with_empty_format(string name)
        {
            Test($"{{{name}:}}", new PropertyToken(name));
        }

        [TestCase("U")]
        [TestCase("hh:mm:ss")]
        [TestCase("hh;mm;ss")]
        [TestCase("hh mm ss")]
        [TestCase("hh,mm,ss")]
        [TestCase("hh.mm.ss")]
        public void Should_produce_single_property_token_for_template_with_a_single_property_with_non_empty_format(string format)
        {
            Test($"{{Hello:{format}}}", new PropertyToken("Hello", format));
        }

        [Test]
        public void Should_treat_doble_left_braces_as_a_single_brace()
        {
            Test("{{ Hi! }", new TextToken("{ Hi! }"));
        }

        [Test]
        public void Should_treat_doble_left_braces_as_a_single_brace_inside_text()
        {
            Test("Well, {{ Hi!", new TextToken("Well, { Hi!"));
        }

        [Test]
        public void Should_parse_double_right_brackets_as_a_single_bracket()
        {
            Test("Nice }}-: mo", new TextToken("Nice }-: mo"));
        }

        [TestCase("{0 space}")]
        [TestCase("{:}")]
        [TestCase("{ }")]
        [TestCase("{:format}")]
        [TestCase("{!:format}")]
        [TestCase("{Hello")]
        [TestCase("{,,}")]
        [TestCase("{}")]
        [TestCase("{")]
        [TestCase("}")]
        public void Should_parse_malformed_named_token_as_text(string value)
        {
            Test(value, new TextToken(value));
        }

        [TestCase("{0 space}")]
        [TestCase("{:}")]
        [TestCase("{:format}")]
        [TestCase("{!:format}")]
        [TestCase("{Hello")]
        [TestCase("{,,}")]
        [TestCase("{}")]
        [TestCase("{")]
        public void Should_parse_malformed_named_token_with_leading_text_as_two_text_tokens(string value)
        {
            const string prefix = "prefix + ";

            Test(prefix + value, new TextToken(prefix), new TextToken(value));
        }

        [TestCase("{0 space}")]
        [TestCase("{:}")]
        [TestCase("{:format}")]
        [TestCase("{!:format}")]
        [TestCase("{,,}")]
        [TestCase("{}")]
        public void Should_parse_malformed_named_token_with_trailing_text_as_two_text_tokens(string value)
        {
            const string suffix = "prefix + ";

            Test(value + suffix, new TextToken(value), new TextToken(suffix));
        }

        [Test]
        public void Should_parse_muliple_named_tokens_without_formats()
        {
            Test("...{Greeting}, {Name}!", 
                new TextToken("..."),
                new PropertyToken("Greeting"),
                new TextToken(", "),
                new PropertyToken("Name"),
                new TextToken("!")
            );
        }

        [Test]
        public void Should_parse_muliple_named_tokens_with_formats()
        {
            Test("...{Greeting:123}, {Name:456}!",
                new TextToken("..."),
                new PropertyToken("Greeting", "123"),
                new TextToken(", "),
                new PropertyToken("Name", "456"),
                new TextToken("!")
            );
        }

        [Test]
        public void Should_parse_a_named_token_surrounded_with_text_on_both_sides()
        {
            Test("aa{prop}bb", new TextToken("aa"), new PropertyToken("prop"), new TextToken("bb"));
        }

        [Test]
        public void Should_parse_a_named_token_surrounded_with_text_on_left_side()
        {
            Test("aa{prop}", new TextToken("aa"),new PropertyToken("prop"));
        }

        [Test]
        public void Should_parse_a_named_token_surrounded_with_text_on_right_side()
        {
            Test("{prop}bb", new PropertyToken("prop"), new TextToken("bb"));
        }

        [Test]
        public void Should_handle_a_template_ending_with_a_left_brace()
        {
            Test("aa{", new TextToken("aa"), new TextToken("{"));
        }

        [Test]
        public void Should_handle_a_template_starting_with_a_right_brace()
        {
            Test("}aa", new TextToken("}aa"));
        }

        [Test]
        public void Should_not_parse_a_named_token_in_double_braces()
        {
            Test("aa{{prop}}bb", new TextToken("aa{prop}bb"));
        }

        [Test]
        public void Should_not_parse_a_named_token_in_double_braces_if_template_starts_with_it()
        {
            Test("{{prop}}bb", new TextToken("{prop}bb"));
        }

        [Test]
        public void Should_not_parse_a_named_token_in_double_braces_if_template_ends_with_it()
        {
            Test("aa{{prop}}", new TextToken("aa{prop}"));
        }

        [Test]
        public void Should_not_parse_a_named_token_if_the_left_brace_is_doubled()
        {
            Test("aa{{prop}bb", new TextToken("aa{prop}bb"));
        }

        [Test]
        public void Should_parse_a_named_token_if_the_right_brace_is_doubled()
        {
            Test("aa{prop}}bb", new TextToken("aa"), new PropertyToken("prop"), new TextToken("}bb"));
        }

        [Test]
        public void Should_parse_a_named_token_with_triple_braces()
        {
            Test("{{{prop}}}", new TextToken("{"), new PropertyToken("prop"), new TextToken("}"));
        }

        [Test]
        public void Should_not_parse_a_named_token_with_inverted_braces()
        {
            Test("aa}prop{bb", new TextToken("aa}prop"), new TextToken("{bb"));
        }

        [Test]
        public void Should_not_parse_a_named_token_with_inverted_braces_when_template_starts_and_ends_with_it()
        {
            Test("}prop{", new TextToken("}prop"), new TextToken("{"));
        }

        [Test]
        public void Should_not_parse_a_named_token_without_right_brace_when_template_starts_with_it()
        {
            Test("{prop", new TextToken("{prop"));
        }

        [Test]
        public void Should_not_parse_a_named_token_without_left_brace()
        {
            Test("prop}bb", new TextToken("prop}bb"));
        }

        [Test]
        public void Should_not_parse_a_named_token_without_left_brace_when_template_ends_with_it()
        {
            Test("prop}", new TextToken("prop}"));
        }

        [Test]
        public void Should_not_parse_a_named_token_when_there_are_nested_braces()
        {
            Test("aa{bb{prop}cc}dd", 
                new TextToken("aa"), 
                new TextToken("{bb{prop}"),
                new TextToken("cc}dd")
            );
        }

        [Test]
        public void Should_parse_a_named_token_after_a_closed_nexted_braces_construct()
        {
            Test("aa{bb{prop}cc}dd{prop2}",
                new TextToken("aa"),
                new TextToken("{bb{prop}"),
                new TextToken("cc}dd"),
                new PropertyToken("prop2")
            );
        }

        [Test]
        public void Should_parse_several_named_tokens_without_text_between_them()
        {
            Test("{prop1}{prop2:f}{prop3}",
                new PropertyToken("prop1"),
                new PropertyToken("prop2", "f"),
                new PropertyToken("prop3")
            );
        }

        [Test]
        public void Should_produce_repeatable_results()
        {
            for (var i = 0; i < 100; i++)
            {
                Test("...{prop1}{prop2:f}{prop3}...{{}}",
                    new TextToken("..."),
                    new PropertyToken("prop1"),
                    new PropertyToken("prop2", "f"),
                    new PropertyToken("prop3"),
                    new TextToken("...{}")
                );
            }
        }

        private static void Test(string template, params ITemplateToken[] expectedTokens)
        {
            var actualTokens = TemplateTokenizer.Tokenize(template, new PropertyTokensFactory()).ToArray();

            actualTokens.Should().HaveCount(expectedTokens.Length);

            foreach (var (actual, expected) in actualTokens.Zip(expectedTokens, (actual, expected) => (actual, expected)))
            {
                if (expected is TextToken expectedText)
                {
                    actual.Should().BeOfType<TextToken>().Which.ToString().Should().Be(expectedText.ToString());
                }
                else if (expected is PropertyToken expectedProperty)
                {
                    actual.Should().BeOfType<PropertyToken>().And.BeEquivalentTo(expectedProperty);
                }
                else throw new InvalidOperationException("Expected tokens can only be TextTokens or PropertyTokens.");
            }
        }
    }
}