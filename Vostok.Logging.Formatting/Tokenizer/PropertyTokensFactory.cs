using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tokenizer
{
    internal class PropertyTokensFactory : INamedTokenFactory
    {
        public ITemplateToken Create(string name, string format) =>
            new PropertyToken(name, format);
    }
}