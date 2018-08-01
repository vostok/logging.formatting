using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    [UsedImplicitly]
    internal class ExceptionToken : NamedToken
    {
        public ExceptionToken([CanBeNull] string format = null)
            : base(TokenNames.Exception, format)
        {
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            if (@event.Exception == null)
                return;

            // TODO(iloktionov): render without using Exception's ToString()

            throw new NotImplementedException();
        }
    }
}
