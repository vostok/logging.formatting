using System.Collections.Generic;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tokenizers
{
    internal class MessageTemplateTokenizer : ITemplateTokenizer
    {
        public IEnumerable<ITemplateToken> Tokenize(string template)
        {
            // TODO(iloktionov): Only use TextToken and PropertyToken here.

            throw new System.NotImplementedException();
        }
    }
}