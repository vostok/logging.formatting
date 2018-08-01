using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    [UsedImplicitly]
    internal class TimestampToken : NamedToken
    {
        public const string DefaultFormat = "yyyy-MM-dd HH:mm:ss,fff";

        public TimestampToken([CanBeNull] string format = null)
            : base(PropertyNames.Timestamp, format ?? DefaultFormat)
        {
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            writer.Write(@event.Timestamp.ToString(Format, formatProvider));
        }
    }
}
