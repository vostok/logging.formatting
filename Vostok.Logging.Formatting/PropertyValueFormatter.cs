using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Commons.Formatting;
using Vostok.Logging.Formatting.Helpers;

namespace Vostok.Logging.Formatting
{
    /// <summary>
    /// <para>Represents a helper used to format log event property values during rendering.</para>
    /// <para>See <see cref="Format(TextWriter,object,string,IFormatProvider)"/> method for details.</para>
    /// </summary>
    [PublicAPI]
    public static class PropertyValueFormatter
    {
        private const int StringBuilderCapacity = 64;

        /// <inheritdoc cref="Format(TextWriter,object,string,IFormatProvider)"/>
        public static string Format(
            [CanBeNull] object value,
            [CanBeNull] string format = null,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            var builder = StringBuilderCache.Acquire(StringBuilderCapacity);
            var writer = new StringWriter(builder);

            Format(writer, value, format, formatProvider);

            StringBuilderCache.Release(builder);

            return builder.ToString();
        }

        /// <summary>
        /// <inheritdoc cref="ObjectValueFormatter.Format(TextWriter,object,string,IFormatProvider)"/>
        /// </summary>
        public static void Format(
            [NotNull] TextWriter writer,
            [CanBeNull] object value,
            [CanBeNull] string format = null,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            if (value == null)
                return;

            if (PaddingFormatHelper.TryParseFormat(format, out var insertLeadingSpace, out var insertTrailingSpace))
                format = null;

            if (insertLeadingSpace)
                writer.WriteSpace();

            ObjectValueFormatter.Format(writer, value, format, formatProvider);

            if (insertTrailingSpace)
                writer.WriteSpace();
        }

        private static void WriteSpace(this TextWriter writer) => writer.Write(" ");
    }
}