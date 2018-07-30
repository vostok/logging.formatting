using System;
using System.IO;
using JetBrains.Annotations;

namespace Vostok.Logging.Formatting.Helpers
{
    internal static class PropertyValueFormatter
    {
        public static void Format(
            [NotNull] TextWriter writer,
            [CanBeNull] object value, 
            [CanBeNull] string format, 
            [CanBeNull] IFormatProvider formatProvider)
        {
            if (value == null)
                return;

            if (value is IFormattable formattable)
            {
                writer.Write(formattable.ToString(format, formatProvider));
                return;
            }

            writer.Write(value.ToString());

            // TODO(iloktionov): Wisely choose between ToString(), IFormattable and JSON here.
        }
    }
}
