using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting.Tokenizers;

namespace Vostok.Logging.Formatting
{
    [PublicAPI]
    public static class LogEventFormatter
    {
        private static readonly ITemplateTokenizer Tokenizer = new EventTemplateTokenizer();

        public static string Format(
            [NotNull] OutputTemplate template,
            [CanBeNull] LogEvent @event,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            // TODO(iloktionov): Just use StringWriter built on a pooled StringBuilder here.

            throw new NotImplementedException();
        }

        public static void Format(
            [NotNull] TextWriter writer,
            [NotNull] OutputTemplate template,
            [CanBeNull] LogEvent @event,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            // TODO(iloktionov): Just render using the template.

            throw new NotImplementedException();
        }
    }
}