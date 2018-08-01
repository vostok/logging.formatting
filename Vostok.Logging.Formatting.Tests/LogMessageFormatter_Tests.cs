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
    }
}