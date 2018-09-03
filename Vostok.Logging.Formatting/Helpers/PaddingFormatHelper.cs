using System.Linq;

namespace Vostok.Logging.Formatting.Helpers
{
    internal static class PaddingFormatHelper
    {
        private const string LeadingSpaceFormat = "W";
        private const string TrailingSpaceFormat = "w";

        private static readonly string[] SpaceFormats =
        {
            TrailingSpaceFormat,
            LeadingSpaceFormat,
            TrailingSpaceFormat + LeadingSpaceFormat,
            LeadingSpaceFormat + TrailingSpaceFormat
        };

        public static bool TryParseFormat(string format, out bool insertLeadingSpace, out bool insertTrailingSpace)
        {
            if (format != null && SpaceFormats.Contains(format))
            {
                insertLeadingSpace = format.Contains(LeadingSpaceFormat);
                insertTrailingSpace = format.Contains(TrailingSpaceFormat);
                return true;
            }

            insertLeadingSpace = false;
            insertTrailingSpace = false;
            return false;
        }
    }
}