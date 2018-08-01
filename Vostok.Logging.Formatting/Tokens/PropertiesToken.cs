﻿using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    [UsedImplicitly]
    internal class PropertiesToken : NamedToken
    {
        public PropertiesToken([CanBeNull] string format = null)
            : base(PropertyNames.Properties, format)
        {
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
    }
}