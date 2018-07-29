using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    internal class TextToken : ITemplateToken
    {
        private readonly string text;
        private readonly int offset;
        private readonly int length;

        public TextToken([NotNull] string text, int offset, int length)
        {
            this.text = text ?? throw new ArgumentNullException(nameof(text));
            this.offset = offset;
            this.length = length;
        }

        public void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            if (length == 0)
                return;

            if (length == text.Length)
            {
                writer.Write(text);
            }
            else
            {
                for (var index = 0; index < length; index++)
                {
                    writer.Write(text[offset + index]);
                }
            }
        }

        public override string ToString() => length == text.Length ? text : text.Substring(offset, length);
    }
}