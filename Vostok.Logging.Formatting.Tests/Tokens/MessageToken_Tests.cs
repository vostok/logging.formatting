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
    internal class MessageToken_Tests
    {
        private MessageToken token;
        private LogEvent @event;

        [SetUp]
        public void TestSetup()
        {
            token = new MessageToken();
            @event = new LogEvent(LogLevel.Info, DateTimeOffset.Now, "Hello, {Name}!").WithProperty("Name", "Vostok");
        }

        [Test]
        public void Render_should_write_formatted_message_to_output()
        {
            Render().Should().Be("Hello, Vostok!");
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