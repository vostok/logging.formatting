using System.Collections.Generic;
using JetBrains.Annotations;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tokenizers
{
    internal interface ITemplateTokenizer
    {
        [NotNull] IEnumerable<ITemplateToken> Tokenize(string template);
    }
}
