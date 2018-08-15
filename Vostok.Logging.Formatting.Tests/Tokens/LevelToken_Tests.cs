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
        public void Should_correctly_render_given_log_level_with_padding_and_default_format(LogLevel level, string expected)
        {
            var @event = new LogEvent(level, DateTimeOffset.Now, null);

            var token = new LevelToken();

            var writer = new StringWriter(new StringBuilder());

            token.Render(@event, writer, null);

            writer.ToString().Should().Be(expected);
        }

        [TestCase(LogLevel.Info, "u1", "I")]
        [TestCase(LogLevel.Info, "w2", "in")]
        [TestCase(LogLevel.Info, "t3", "Inf")]
        [TestCase(LogLevel.Info, "u4", "INFO")]
        [TestCase(LogLevel.Info, "t5", "Info ")]
        [TestCase(LogLevel.Info, null, "INFO ")]
        [TestCase(LogLevel.Info, "t", "INFO ")]
        [TestCase(LogLevel.Info, "u11", "INFO ")]
        [TestCase(LogLevel.Info, "t7", "Info ")]
        [TestCase(LogLevel.Info, "w0", "info ")]
        [TestCase(LogLevel.Error, "w5", "error")]
        [TestCase(LogLevel.Debug, "t5", "Debug")]
        [TestCase(LogLevel.Fatal, "u5", "FATAL")]
        [TestCase(LogLevel.Warn, "t5", "Warn ")]
        public void Should_correctly_render_with_given_format(LogLevel level, string format, string expected)
        {
            var @event = new LogEvent(level, DateTimeOffset.Now, null);
            var token = new LevelToken(format);
            var writer = new StringWriter(new StringBuilder());

            token.Render(@event, writer, null);

            writer.ToString().Should().Be(expected);
        }
    }
}