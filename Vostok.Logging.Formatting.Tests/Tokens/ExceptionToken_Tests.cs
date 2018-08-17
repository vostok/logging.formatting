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
    internal class ExceptionToken_Tests
    {
        private ExceptionToken token;

        [SetUp]
        public void TestSetup()
        {
            token = new ExceptionToken();
        }

        [Test]
        public void Should_not_render_anything_for_null_exception()
        {
            Format(null).Should().BeEmpty();
        }

        [Test]
        public void Should_correctly_format_type_and_messages_for_exceptions_without_nested_exceptions()
        {
            try
            {
                throw new Exception("error1");
            }
            catch (Exception error)
            {
                var result = Format(error);

                System.Console.Out.WriteLine(result);

                result.Should().StartWith("System.Exception: error1");
            }
        }

        [Test]
        public void Should_correctly_format_type_and_messages_for_exceptions_with_nested_exceptions()
        {
            try
            {
                throw new Exception("error1", new Exception("error2", new FormatException("error3")));
            }
            catch (Exception error)
            {
                var result = Format(error);

                System.Console.Out.WriteLine(result);

                result.Should().StartWith("System.Exception: error1 ---> System.Exception: error2 ---> System.FormatException: error3");
            }
        }

        private string Format(Exception error)
        {
            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);

            var logEvent = new LogEvent(LogLevel.Error, DateTimeOffset.Now, null, error);

            token.Render(logEvent, stringWriter, null);

            return stringBuilder.ToString();
        }
    }
}