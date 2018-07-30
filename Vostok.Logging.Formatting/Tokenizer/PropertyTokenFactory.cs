﻿using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tokenizer
{
    internal class PropertyTokenFactory : INamedTokenFactory
    {
        public ITemplateToken Create(string name, string format)
        {
            return new PropertyToken(name, format);
        }
    }
}