using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting
{
    [PublicAPI]
    public static class LogMessageFormatter
    {
        public static string Format(
            [NotNull] LogEvent @event,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            // TODO(iloktionov): Just use StringWriter built on a pooled StringBuilder here.

            throw new NotImplementedException();
        }

        public static void Format(
            [NotNull] LogEvent @event,
            [NotNull] TextWriter writer,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            // TODO(iloktionov): 1. Lookup a template in cache.
            // TODO(iloktionov): 2. If there's nothing in cache, tokenize the template and attempt to save the result to cache.
            // TODO(iloktionov): 3. Just render using the template.

            throw new NotImplementedException();
        }
    }
}
