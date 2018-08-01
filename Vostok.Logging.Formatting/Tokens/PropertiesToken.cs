using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    // TODO(iloktionov): Do not render properties present elsewhere in the output template.

    [UsedImplicitly]
    internal class PropertiesToken : NamedToken
    {
        public PropertiesToken([CanBeNull] string format = null)
            : base(PropertyNames.Properties, format)
        {
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            var properties = @event.Properties;
            if (properties == null || properties.Count == 0)
                return;

            PropertyValueFormatter.Format(writer, @event.Properties, Format, formatProvider);
        }
    }
}