﻿using System;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tests
{
    [TestFixture]
    internal class LogEventFormatter_Tests
    {
        [Test]
        public void Should_correctly_render_events_using_provided_template()
        {
            var template = OutputTemplate.Default;

            var @event = new LogEvent(LogLevel.Warn, DateTimeOffset.Now, "Hello, {User}!")
                .WithProperty("User", "Vostok");

            var result = LogEventFormatter.Format(@event, template);

            result.Should().Be($"{@event.Timestamp.ToString(TimestampToken.DefaultFormat)} WARN  Hello, Vostok!" + Environment.NewLine);

            Console.Out.WriteLine(result);
        }
    }
}