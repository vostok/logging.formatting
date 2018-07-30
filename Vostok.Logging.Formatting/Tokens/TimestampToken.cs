﻿using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    [UsedImplicitly]
    internal class TimestampToken : NamedToken
    {
        public TimestampToken([CanBeNull] string format = null)
            : base(TokenNames.Timestamp, format)
        {
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            writer.Write(@event.Timestamp.ToString(Format, formatProvider));
        }
    }
}