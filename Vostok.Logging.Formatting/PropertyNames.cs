using JetBrains.Annotations;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Vostok.Logging.Formatting
{
    /// <summary>
    /// Defines names of special well-known properties in for <see cref="OutputTemplate"/>.
    /// </summary>
    [PublicAPI]
    public static class PropertyNames
    {
        public const string Exception = "Exception";

        public const string Level = "Level";

        public const string Message = "Message";

        public const string NewLine = "NewLine";

        public const string Properties = "Properties";

        public const string Timestamp = "Timestamp";

        public const string Uptime = "Uptime";

        public const string ContextualPrefix = "ContextualPrefix";
    }
}