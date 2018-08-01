﻿using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    [UsedImplicitly]
    internal class MessageToken : NamedToken
    {
        public MessageToken([CanBeNull] string format = null)
            : base(PropertyNames.Message, format)
        {
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            LogMessageFormatter.Format(@event, writer, formatProvider);
        }
    }
}