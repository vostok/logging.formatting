using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting
{
    [PublicAPI]
    public class OutputTemplate
    {
        [NotNull]
        public static OutputTemplateBuilder Create() => new OutputTemplateBuilder();

        [CanBeNull]
        public static OutputTemplate Parse(string input) => throw new NotImplementedException();

        private readonly IList<ITemplateToken> tokens;

        internal OutputTemplate(IList<ITemplateToken> tokens)
        {
            this.tokens = tokens;
        }

        public override string ToString() => throw new NotImplementedException();
    }
}
