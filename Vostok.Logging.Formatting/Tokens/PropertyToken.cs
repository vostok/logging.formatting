﻿using System;
using System.IO;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    internal class PropertyToken : ITemplateToken
    {
        public void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
    }
}