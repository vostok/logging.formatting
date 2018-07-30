using System;
using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tests.Tokens
{
    [TestFixture]
    internal class LevelToken_Tests
    {
        [TestCase(LogLevel.Debug, "DEBUG")]
        [TestCase(LogLevel.Info,  "INFO ")]
        [TestCase(LogLevel.Warn,  "WARN ")]
        [TestCase(LogLevel.Error, "ERROR")]
        [TestCase(LogLevel.Fatal, "FATAL")]
        public void Should_correctly_render_given_log_level_with_padding(LogLevel level, string expected)
        {
            var @event = new LogEvent(level, DateTimeOffset.Now, null);

            var token = new LevelToken();

            var writer = new StringWriter(new StringBuilder());

            token.Render(@event, writer, null);

            writer.ToString().Should().Be(expected);
        }
    }
}