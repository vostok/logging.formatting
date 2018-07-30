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
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Offset = offset;
            Length = length;
        }

        public TextToken([NotNull] string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Offset = 0;
            Length = text.Length;
        }

        public string Text { get; }

        public int Offset { get; }

        public int Length { get; }

        public void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            if (Length == 0)
                return;

            if (Length == Text.Length)
            {
                writer.Write(Text);
            }
            else
            {
                for (var index = 0; index < Length; index++)
                {
                    writer.Write(Text[Offset + index]);
                }
            }
        }

        public override string ToString() => Length == Text.Length ? Text : Text.Substring(Offset, Length);
    }
}
