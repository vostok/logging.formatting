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
            : base(TokenNames.Message, format)
        {
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
    }
}