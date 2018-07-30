using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting.Tokenizer;

namespace Vostok.Logging.Formatting
{
    [PublicAPI]
    public static class LogMessageFormatter
    {
        private static readonly INamedTokenFactory TokenFactory = new PropertyTokensFactory();

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
            // TODO(iloktionov): template caching

            foreach (var token in TemplateTokenizer.Tokenize(@event.MessageTemplate, TokenFactory))
            {
                token.Render(@event, writer, formatProvider);
            }
        }
    }
}
