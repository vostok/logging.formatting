using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    [UsedImplicitly]
    internal class LevelToken : NamedToken
    {
        private static readonly Dictionary<LogLevel, string> RenderedLevels;

        static LevelToken()
        {
            var maxNameLength = Enum.GetNames(typeof(LogLevel)).Max(name => name.Length);

            RenderedLevels = new Dictionary<LogLevel, string>();

            foreach (var level in Enum.GetValues(typeof (LogLevel)).Cast<LogLevel>())
                RenderedLevels.Add(level, level.ToString().ToUpperInvariant().PadRight(maxNameLength));
        }

        public LevelToken([CanBeNull] string format = null)
            : base(TokenNames.Level, format)
        {
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            writer.Write(RenderedLevels.TryGetValue(@event.Level, out var rendered) ? rendered : @event.Level.ToString());
        }
    }
}
