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

        private const int MinimumYearToPrerender = 2018;
        private const int TotalYearsToPrerender = 100;

        private static readonly IFormatProvider InvariantCulture = CultureInfo.InvariantCulture;

        private static readonly string[] PrerenderedYears = PrerenderNumbers(MinimumYearToPrerender, TotalYearsToPrerender, 4);
        private static readonly string[] PrerenderedUpToSeconds = PrerenderNumbers(0, 60, 2);
        private static readonly string[] PrerenderedMilliseconds = PrerenderNumbers(0, 1000, 3);

        private readonly bool hasCustomFormat;

        public TimestampToken([CanBeNull] string format = null)
            : base(WellKnownTokens.Timestamp, format)
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FormatTimestampEfficiently(DateTime timestamp, TextWriter writer)
        {
            writer.Write(RenderYear(timestamp.Year));
            writer.Write('-');

            writer.Write(PrerenderedUpToSeconds[timestamp.Month]);
            writer.Write('-');

            writer.Write(PrerenderedUpToSeconds[timestamp.Day]);
            writer.Write(' ');

            writer.Write(PrerenderedUpToSeconds[timestamp.Hour]);
            writer.Write(':');

            writer.Write(PrerenderedUpToSeconds[timestamp.Minute]);
            writer.Write(':');

            writer.Write(PrerenderedUpToSeconds[timestamp.Second]);
            writer.Write(',');

            writer.Write(PrerenderedMilliseconds[timestamp.Millisecond]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string RenderYear(int year)
        {
            var prerenderedIndex = year - MinimumYearToPrerender;
            if (prerenderedIndex < 0 || prerenderedIndex >= TotalYearsToPrerender)
                return year.ToString(InvariantCulture).PadLeft(4, '0');

            return PrerenderedYears[prerenderedIndex];
        }

        private static string[] PrerenderNumbers(int from, int count, int desiredLength)
        {
            var result = new string[count];

            for (var i = 0; i < count; i++)
            {
                result[i] = (from + i).ToString(InvariantCulture).PadLeft(desiredLength, '0');
            }

            return result;
        }
    }
}