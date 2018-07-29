using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    internal interface ITemplateToken
    {
        void Render([NotNull] LogEvent @event, [NotNull] TextWriter writer, [CanBeNull] IFormatProvider formatProvider);
    }
}
