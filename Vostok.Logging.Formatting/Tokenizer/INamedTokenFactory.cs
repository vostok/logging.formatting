using JetBrains.Annotations;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tokenizer
{
    internal interface INamedTokenFactory
    {
        [NotNull]
        ITemplateToken Create([NotNull] string name, [CanBeNull] string format);
    }
}