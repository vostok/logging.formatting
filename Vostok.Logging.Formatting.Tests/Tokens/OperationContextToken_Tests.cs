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
    internal class OperationContextToken_Tests
    {
        private OperationContextToken token;
        private LogEvent @event;

        [SetUp]
        public void TestSetup()
        {
            token = new OperationContextToken();
            @event = new LogEvent(LogLevel.Info, DateTimeOffset.Now, "Hello, {Name}!").WithProperty("Name", "Vostok");
        }

        [Test]
        public void Should_render_nothing_for_event_without_operation_context_property()
        {
            Render().Should().BeEmpty();
        }
        
        [Test]
        public void Should_render_nothing_for_event_with_null_operation_context_property()
        {
            @event = @event.WithProperty(WellKnownProperties.OperationContext, (string)null);
            Render().Should().BeEmpty();
        }

        [Test]
        public void Should_not_insert_surrounding_spaces_around_missing_operation_context_property()
        {
            token = new OperationContextToken("wW");

            Render().Should().BeEmpty();
        }
        
        [Test]
        public void Should_render_value_of_existing_operation_context_property_using_provided_format()
        {
            token = new OperationContextToken("D5");
            @event = @event.WithProperty(WellKnownProperties.OperationContext, 1);

            Render().Should().Be("00001");
        }
        
        [Test]
        public void Render_should_render_template()
        {
            @event = @event.WithProperty(WellKnownProperties.OperationContext, "Operation {Name}.");
            Render().Should().Be("Operation Vostok.");
        }
        
        [Test]
        public void Render_should_render_template_with_objects()
        {
            @event = @event
                .WithProperty(WellKnownProperties.OperationContext, "Operation {Name}.")
                .WithProperty("Name", new {X = 43, Y = "yy"});
            Render().Should().Be(@"Operation {""X"": ""43"", ""Y"": ""yy""}.");
        }
        
        [Test]
        public void Render_should_render_template_with_spaces_format()
        {
            token = new OperationContextToken("wW");
            @event = @event.WithProperty(WellKnownProperties.OperationContext, "Operation {Name}.");
            Render().Should().Be(" Operation Vostok. ");
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