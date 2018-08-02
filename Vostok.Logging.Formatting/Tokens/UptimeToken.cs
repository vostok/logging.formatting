using System;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    [UsedImplicitly]
    internal class UptimeToken : NamedToken
    {
        private static readonly Stopwatch Watch;

        static UptimeToken()
        {
            Watch = Stopwatch.StartNew();
        }

        public UptimeToken([CanBeNull] string format = null)
            : base(PropertyNames.Uptime, format)
        {
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider) =>
            writer.Write(Watch.ElapsedMilliseconds.ToString(Format));
    }
}