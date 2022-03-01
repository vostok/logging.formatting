using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens;

internal class OperationContextToken : NamedToken
{
    public OperationContextToken([CanBeNull] string format = null)
        : base(WellKnownProperties.OperationContext, format)
    {
    }

    public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
    {
        if (@event.Properties == null)
            return;

        if (!@event.Properties.TryGetValue(Name, out var value))
            return;

        OperationContextValueFormatter.Format(@event, writer, value, Format, formatProvider);
    }
}