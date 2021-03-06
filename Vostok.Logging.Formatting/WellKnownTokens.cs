﻿using JetBrains.Annotations;

namespace Vostok.Logging.Formatting
{
    /// <summary>
    /// Defines names of special well-known tokens that can be used in <see cref="OutputTemplate"/>s.
    /// </summary>
    [PublicAPI]
    public static class WellKnownTokens
    {
        public const string Exception = "Exception";

        public const string Level = "Level";

        public const string Message = "Message";

        public const string NewLine = "NewLine";

        public const string Properties = "Properties";

        public const string Timestamp = "Timestamp";

        public const string Uptime = "Uptime";
    }
}