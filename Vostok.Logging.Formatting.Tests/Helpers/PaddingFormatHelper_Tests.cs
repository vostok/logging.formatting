using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Formatting.Helpers;

namespace Vostok.Logging.Formatting.Tests.Helpers
{
    [TestFixture]
    internal class PaddingFormatHelper_Tests
    {
        [TestCase("w", false, true)]
        [TestCase("W", true, false)]
        [TestCase("wW", true, true)]
        [TestCase("Ww", true, true)]
        public void TryParseFormat_should_correctly_parse_valid_formats(
            string format, 
            bool leadingSpaceExpected, 
            bool trailingSpaceExpected)
        {
            PaddingFormatHelper.TryParseFormat(format, out var insertLeadingSpace, out var insertTrailingSpace).Should().BeTrue();

            insertLeadingSpace.Should().Be(leadingSpaceExpected);

            trailingSpaceExpected.Should().Be(insertTrailingSpace);
        }

        [TestCase("invalid!")]
        [TestCase("000")]
        [TestCase("")]
        [TestCase(null)]
        public void TryParseFormat_should_return_false_for_invalid_formats(string format)
        {
            PaddingFormatHelper.TryParseFormat(format, out _, out _).Should().BeFalse();
        }
    }
}