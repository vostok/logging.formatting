using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Formatting.Tokenizers;

namespace Vostok.Logging.Formatting
{
    [PublicAPI]
    public static class LogMessageFormatter
    {
        private static readonly ITemplateTokenizer Tokenizer = new MessageTemplateTokenizer();

        public static string Format(
            [CanBeNull] string template,
            [CanBeNull] IReadOnlyDictionary<string, object> properties,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            // TODO(iloktionov): Just use StringWriter built on a pooled StringBuilder here.

            throw new NotImplementedException();
        }

        public static void Format(
            [NotNull] TextWriter writer,
            [CanBeNull] string template,
            [CanBeNull] IReadOnlyDictionary<string, object> properties,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            // TODO(iloktionov): 1. Lookup a template in cache.
            // TODO(iloktionov): 2. If there's nothing in cache, tokenize the template and attempt to save the result to cache.
            // TODO(iloktionov): 3. Just render using the template.

            throw new NotImplementedException();
        }
    }
}
