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
    internal class PropertiesToken_Tests
    {
        private PropertiesToken token;

        [SetUp]
        public void TestSetup()
        {
            token = new PropertiesToken();
        }

        [Test]
        public void Should_not_render_anything_when_log_event_has_null_properties()
        {
            var logEvent = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null);

            Format(logEvent).Should().BeEmpty();
        }

        [Test]
        public void Should_not_render_anything_when_log_event_has_empty_properties()
        {
            var logEvent = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null)
                .WithProperty("prop", "value")
                .WithoutProperty("prop");

            Format(logEvent).Should().BeEmpty();
        }

        [Test]
        public void Should_format_all_log_event_properties_as_json()
        {
            var logEvent = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null)
                .WithProperty("A", 1)
                .WithProperty("B", 2)
                .WithProperty("C", 3);

            Format(logEvent).Should().Be("{\"A\": \"1\", \"B\": \"2\", \"C\": \"3\"}");
        }

        private string Format(LogEvent logEvent)
        {
            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);

            token.Render(logEvent, stringWriter, null);

            return stringBuilder.ToString();
        }
    }
}