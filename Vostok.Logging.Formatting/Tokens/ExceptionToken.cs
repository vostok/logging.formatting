using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    internal class ExceptionToken : NamedToken
    {
        public ExceptionToken([CanBeNull] string format = null)
            : base("Exception", format)
        {
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            // TODO(iloktionov): render without using Exception's ToString()

            throw new NotImplementedException();
        }
    }
}
