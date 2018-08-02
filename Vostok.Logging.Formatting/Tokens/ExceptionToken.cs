using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    [UsedImplicitly]
    internal class ExceptionToken : NamedToken
    {
        private const int MaximumDepth = 10;

        public ExceptionToken([CanBeNull] string format = null)
            : base(PropertyNames.Exception, format)
        {
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            RenderException(@event.Exception, writer, 0);

            if (@event.Exception != null)
                writer.WriteLine();
        }

        private static void RenderException(Exception error, TextWriter writer, int depth)
        {
            if (error == null)
                return;

            if (depth > MaximumDepth)
            {
                writer.Write("<too deep>");
                return;
            }

            writer.Write(error.GetType().ToString());

            var errorMessage = error.Message;
            if (errorMessage != null)
            {
                writer.Write(": ");
                writer.Write(errorMessage);
            }

            var innerError = error.InnerException;
            if (innerError != null)
            {
                writer.Write(" ---> ");

                RenderException(innerError, writer, depth + 1);

                writer.WriteLine();
                writer.Write("   ");
                writer.Write("--- End of inner exception stack trace ---");
            }

            var stackTrace = error.StackTrace;
            if (stackTrace != null)
            {
                writer.WriteLine();
                writer.Write(stackTrace);
            }
        }
    }
}
