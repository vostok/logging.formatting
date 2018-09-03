using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    internal class TextToken : ITemplateToken
    {
        public TextToken([NotNull] string text, int offset, int length)
        {
            Text = (text ?? throw new ArgumentNullException(nameof(text))).Substring(offset, length);
        }

        public TextToken([NotNull] string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public string Text { get; }

        public void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            if (Text.Length > 0)
            {
                writer.Write(Text);
            }
        }

        public override string ToString() => Text;
    }
}