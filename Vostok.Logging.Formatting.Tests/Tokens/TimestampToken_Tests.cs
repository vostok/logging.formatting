using System;
using System.Globalization;
using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tests.Tokens
{
    [TestFixture]
    internal class TimestampToken_Tests
    {
        private LogEvent @event;
        private StringWriter writer;

        [SetUp]
        public void TestSetup()
        {
            @event = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null);
            writer = new StringWriter(new StringBuilder());
        }

        [Test]
        public void Should_render_event_timestamp_without_format_using_default_format()
        {
            new TimestampToken().Render(@event, writer, null);

            System.Console.Out.WriteLine(writer.ToString());

            writer.ToString().Should().Be(@event.Timestamp.ToString(TimestampToken.DefaultFormat));
        }

        [Test]
        public void Should_render_event_timestamp_with_format_correctly()
        {
            const string format = "yyyy MM dd HH-mm-ss";

            new TimestampToken(format).Render(@event, writer, null);

            System.Console.Out.WriteLine(writer.ToString());

            writer.ToString().Should().Be(@event.Timestamp.ToString(format));
        }

        [Test]
        public void Should_render_event_timestamp_with_format_provider_correctly()
        {
            var formatProvider = CultureInfo.InvariantCulture;

            new TimestampToken().Render(@event, writer, formatProvider);

            System.Console.Out.WriteLine(writer.ToString());

            writer.ToString().Should().Be(@event.Timestamp.ToString(TimestampToken.DefaultFormat, formatProvider));
        }

        [Test]
        public void Should_render_event_timestamp_with_format_and_format_provider_correctly()
        {
            const string format = "G";

            var formatProvider = CultureInfo.InvariantCulture;

            new TimestampToken(format).Render(@event, writer, formatProvider);

            System.Console.Out.WriteLine(writer.ToString());

            writer.ToString().Should().Be(@event.Timestamp.ToString(format, formatProvider));
        }
    }
}