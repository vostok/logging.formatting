using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    internal class ExceptionToken : NamedToken
    {
        private const int MaximumDepth = 10;

        public ExceptionToken([CanBeNull] string format = null)
            : base(WellKnownTokens.Exception, format)
        {
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            if (@event.Exception == null)
                return;

            writer.WriteLine(@event.Exception.ToString());
        }
    }
}