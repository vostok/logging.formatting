using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    internal class PropertyToken : NamedToken
    {
        public PropertyToken([NotNull] string name, [CanBeNull] string format = null)
            : base(name, format)
        {
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            if (@event.Properties == null)
                return;

            if (@event.Properties.TryGetValue(Name, out var value))
            {
                PropertyValueFormatter.Format(writer, value, Format, formatProvider);
            }
        }
    }
}