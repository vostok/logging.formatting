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
        public void Should_use_ToString_to_render_exception()
        {
            try
            {
                throw new Exception("error1", new Exception("error2"));
            }
            catch (Exception error)
            {
                var result = Format(error);

                Console.Out.WriteLine(result);

                result.Should()
                    .Contain(error.ToString())
                    .And.Contain(typeof(Exception).FullName)
                    .And.Contain("error1")
                    .And.Contain("error2")
                    .And.Contain(nameof(Should_use_ToString_to_render_exception));
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