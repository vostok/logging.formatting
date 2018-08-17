using System;
using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting.Tokens;
using ConLog = Vostok.Logging.Console;

namespace Vostok.Logging.Formatting.Tests.Tokens
{
    [TestFixture]
    internal class PropertyToken_Tests
    {
        private PropertyToken token;
        private LogEvent @event;

        [SetUp]
        public void TestSetup()
        {
            token = new PropertyToken("prop", "D5");
            @event = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null);
        }

        [Test]
        public void Should_render_nothing_for_event_without_properties()
        {
            Render().Should().BeEmpty();
        }

        [Test]
        public void Should_render_nothing_for_event_without_needed_property()
        {
            @event = @event
                .WithProperty("anotherProp1", 1)
                .WithProperty("anotherProp2", 2);

            Render().Should().BeEmpty();
        }

        [Test]
        public void Should_render_value_of_existing_property_using_provided_format()
        {
            @event = @event.WithProperty("prop", 1);

            Render().Should().Be("00001");
        }

        [TestCase("w", "1 ")]
        [TestCase("W", " 1")]
        [TestCase("wW", " 1 ")]
        [TestCase("D2", "01")]
        [TestCase("wWD2", " 01 ")]
        [TestCase("D2w", "01 ")]
        [TestCase("DW2", " 01")]
        public void Should_render_correctly_using_format_for_spaces(string format, string expected)
        {
            @event = @event.WithProperty("prop", 1);
            var formattedToken = new PropertyToken("prop", format);
            Render(formattedToken).Should().Be(expected);
        }

        private string Render(PropertyToken anotherToken = null)
        {
            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);

            (anotherToken ?? token).Render(@event, stringWriter, null);

            return stringBuilder.ToString();
        }
    }
}