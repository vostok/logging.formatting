using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting
{
    [PublicAPI]
    public static class LogEventFormatter
    {
        public static void Format(
            [NotNull] LogEvent @event,
            [NotNull] TextWriter writer,
            [NotNull] OutputTemplate template,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            // TODO(iloktionov): Just render using the template.

            throw new NotImplementedException();
        }
    }
}