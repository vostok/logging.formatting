using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tokenizer
{
    internal static class TemplateTokenizer
    {
        [NotNull]
        public static IEnumerable<ITemplateToken> Tokenize([CanBeNull] string template, [NotNull] INamedTokenFactory namedTokenFactory)
        {
            if (string.IsNullOrEmpty(template))
                yield break;

            var nextIndex = 0;

            while (true)
            {
                var text = ConsumeText(template, nextIndex, out nextIndex);
                if (text != null)
                    yield return text;

                if (nextIndex == template.Length)
                    yield break;

                var namedToken = ParseNamedToken(template, nextIndex, out nextIndex, namedTokenFactory);
                if (namedToken != null)
                    yield return namedToken;

                if (nextIndex == template.Length)
                    yield break;
            }
        }

        [CanBeNull]
        private static ITemplateToken ConsumeText(string template, int offset, out int next)
        {
            throw new NotImplementedException();
        }

        [CanBeNull]
        private static ITemplateToken ParseNamedToken(string template, int offset, out int next, INamedTokenFactory factory)
        {
            throw new NotImplementedException();
        }
    }
}