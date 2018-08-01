using JetBrains.Annotations;

namespace Vostok.Logging.Formatting.Tokens
{
    /// <summary>
    /// Defines names of special well-known properties in for <see cref="OutputTemplate"/>.
    /// </summary>
    [PublicAPI]
    public static class TokenNames
    {
        public const string Exception = "Exception";

        public const string Level = "Level";

        public const string Message = "Message";

        public const string NewLine = "NewLine";

        public const string Properties = "Properties";

        public const string Timestamp = "Timestamp";

        public const string Uptime = "Uptime";

        public const string Prefix = "Prefix";
    }
}