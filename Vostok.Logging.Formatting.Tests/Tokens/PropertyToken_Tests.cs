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
        public void Should_not_insert_surrounding_spaces_around_missing_property()
        {
            token = new PropertyToken("prop", "wW");

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

        private string Render()
        {
            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);

            token.Render(@event, stringWriter, null);

            return stringBuilder.ToString();
        }
    }
}