using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    [UsedImplicitly]
    internal class TimestampToken : NamedToken
    {
        public const string DefaultFormat = "yyyy-MM-dd HH:mm:ss,fff";

        private static readonly IFormatProvider InvariantCulture = CultureInfo.InvariantCulture;

        private readonly bool hasCustomFormat;

        public TimestampToken([CanBeNull] string format = null)
            : base(PropertyNames.Timestamp, format)
        {
            hasCustomFormat = Format != null && Format != DefaultFormat;
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            var hasCustomFormatProvider = formatProvider != null && !ReferenceEquals(formatProvider, InvariantCulture);

            if (hasCustomFormat || hasCustomFormatProvider)
            {
                writer.Write(@event.Timestamp.ToString(Format, formatProvider));
            }
            else
            {
                FormatTimestampEfficiently(@event.Timestamp.DateTime, writer);
            }
        }

        // TODO(krait): Benchmark.
        private static void FormatTimestampEfficiently(DateTime timestamp, TextWriter writer)
        {
            writer.Write(timestamp.Year.ToString(InvariantCulture));
            writer.Write('-');

            WriteNumberWithTwoDigitPadding(timestamp.Month, writer);
            writer.Write('-');

            WriteNumberWithTwoDigitPadding(timestamp.Day, writer);
            writer.Write(' ');

            WriteNumberWithTwoDigitPadding(timestamp.Hour, writer);
            writer.Write(':');

            WriteNumberWithTwoDigitPadding(timestamp.Minute, writer);
            writer.Write(':');

            WriteNumberWithTwoDigitPadding(timestamp.Second, writer);
            writer.Write(',');
            WriteNumberWithThreeDigitPadding(timestamp.Millisecond, writer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteNumberWithTwoDigitPadding(int number, TextWriter writer)
        {
            if (number < 10)
            {
                writer.Write('0');
            }

            writer.Write(number.ToString(InvariantCulture));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteNumberWithThreeDigitPadding(int number, TextWriter writer)
        {
            if (number < 10)
            {
                writer.Write("00");
            }
            else if (number < 100)
            {
                writer.Write('0');
            }

            writer.Write(number.ToString(InvariantCulture));
        }
    }
}