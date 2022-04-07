using System;
using System.Linq;
using JetBrains.Annotations;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting
{
    [PublicAPI]
    public static class OutputTemplateExtensions
    {
        [Pure]
        public static OutputTemplate WithPropertyAfter([NotNull] this OutputTemplate template, [CanBeNull] string insertAfter, [NotNull] string property, [CanBeNull] string format = null)
        {
            var tokens = template.Tokens.ToList();
            var position = tokens.FindIndex(t => t is NamedToken nt && nt.Name == insertAfter);
            if (insertAfter != null && position == -1)
                throw new InvalidOperationException($"Template '{template}' has no '{insertAfter}' token.");
            
            tokens.Insert(position + 1, new PropertyToken(property, format));

            return new OutputTemplate(tokens);
        }
    }
}