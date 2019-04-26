using System;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tests
{
    [TestFixture]
    internal class LogMessageFormatter_Tests
    {
        [Test]
        public void Should_correctly_format_log_event_message_using_its_properties_and_produce_repeatable_results()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.Now, "Hello, {User}! You have {UnreadCount:D5} messages to read.")
                .WithProperty("User", "Kontur")
                .WithProperty("UnreadCount", 50);

            for (var i = 0; i < 10; i++)
            {
                LogMessageFormatter.Format(@event).Should().Be("Hello, Kontur! You have 00050 messages to read.");
            }
        }

        [Test]
        public void Should_ignore_leading_at_character_in_templates()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.Now, "Hello, {@User}! You have {@UnreadCount:D5} messages to read.")
                .WithProperty("User", "Kontur")
                .WithProperty("UnreadCount", 50);

            LogMessageFormatter.Format(@event).Should().Be("Hello, Kontur! You have 00050 messages to read.");
        }

        [Test]
        public void Should_respect_case_sensitivity_in_templates()
        {
            var event1 = new LogEvent(LogLevel.Info, DateTimeOffset.Now, "Hello, {User}!").WithProperty("User", "Kontur");
            var event2 = new LogEvent(LogLevel.Info, DateTimeOffset.Now, "HELLO, {User}!").WithProperty("User", "Kontur");

            LogMessageFormatter.Format(event1).Should().Be("Hello, Kontur!");
            LogMessageFormatter.Format(event2).Should().Be("HELLO, Kontur!");
        }

        [Test]
        public void Should_support_null_message_template()
        {
            var event1 = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null);

            LogMessageFormatter.Format(event1).Should().BeEmpty();
        }
    }
}