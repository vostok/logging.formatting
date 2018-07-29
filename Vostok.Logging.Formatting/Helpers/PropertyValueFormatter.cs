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
            // TODO(iloktionov): Wisely choose between ToString(), IFormattable and JSON here.

            throw new NotImplementedException();
        }
    }
}
