using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    internal class PropertiesToken : NamedToken
    {
        private readonly ISet<string> exceptions;

        public PropertiesToken([CanBeNull] string format = null)
            : base(WellKnownTokens.Properties, format)
        {
        }

        public PropertiesToken(ISet<string> exceptions, string format)
            : this(format)
        {
            this.exceptions = exceptions;
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            var properties = @event.Properties;
            if (properties == null || properties.Count == 0)
                return;

            PropertyValueFormatter.Format(writer, FilterProperties(properties), Format, formatProvider);
        }

        [NotNull]
        private IReadOnlyDictionary<string, object> FilterProperties([NotNull] IReadOnlyDictionary<string, object> properties)
        {
            if (exceptions == null || exceptions.Count == 0)
                return properties;

            if (!properties.Any(prop => exceptions.Contains(prop.Key)))
                return properties;

            var filteredProperties = new Dictionary<string, object>();

            foreach (var prop in properties)
            {
                if (exceptions.Contains(prop.Key))
                    continue;

                filteredProperties[prop.Key] = prop.Value;
            }

            return filteredProperties;
        }
    }
}