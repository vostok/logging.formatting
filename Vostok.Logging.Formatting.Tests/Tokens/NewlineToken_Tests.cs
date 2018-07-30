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
    internal class NewlineToken_Tests
    {
        [Test]
        public void Should_just_emit_platform_specific_newline_on_render()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null);

            var token = new NewlineToken();

            var writer = new StringWriter(new StringBuilder());

            token.Render(@event, writer, null);

            writer.ToString().Should().Be(Environment.NewLine);
        }
    }
}