using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    [UsedImplicitly]
    internal class NewlineToken : NamedToken
    {
        public NewlineToken([CanBeNull] string format = null)
            : base(PropertyNames.NewLine, format)
        {
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            writer.WriteLine();
        }
    }
}