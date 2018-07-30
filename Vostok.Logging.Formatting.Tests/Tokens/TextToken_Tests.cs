using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tests.Tokens
{
    [TestFixture]
    internal class TextToken_Tests
    {
        [TestCase("What a lovely day!", 0, 18, "What a lovely day!")]
        [TestCase("What a lovely day!", 1, 17, "hat a lovely day!")]
        [TestCase("What a lovely day!", 0, 17, "What a lovely day")]
        [TestCase("What a lovely day!", 5, 9, "a lovely ")]
        public void Should_render_correctly_with_give_offset_and_length(string text, int offset, int length, string expected)
        {
            var writer = new StringWriter();

            var token = new TextToken(text, offset, length);

            token.Render(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, null), writer, null);

            token.ToString().Should().Be(expected);

            writer.ToString().Should().Be(expected);
        }
    }
}