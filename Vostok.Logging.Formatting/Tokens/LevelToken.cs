using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    internal class LevelToken : NamedToken
    {
        private const string DefaultFormat = "u5";
        private const int DefaultWidth = 5;

        private static readonly string[][] TitleCaseLevelMap =
        {
            new[] {"D", "De", "Dbg", "Dbug", "Debug"},
            new[] {"I", "In", "Inf", "Info", "Info "},
            new[] {"W", "Wn", "Wrn", "Warn", "Warn "},
            new[] {"E", "Er", "Err", "Eror", "Error"},
            new[] {"F", "Fa", "Ftl", "Fatl", "Fatal"}
        };

        private static readonly string[][] LowercaseLevelMap =
        {
            new[] {"d", "de", "dbg", "dbug", "debug"},
            new[] {"i", "in", "inf", "info", "info "},
            new[] {"w", "wn", "wrn", "warn", "warn "},
            new[] {"e", "er", "err", "eror", "error"},
            new[] {"f", "fa", "ftl", "fatl", "fatal"}
        };

        private static readonly string[][] UppercaseLevelMap =
        {
            new[] {"D", "DE", "DBG", "DBUG", "DEBUG"},
            new[] {"I", "IN", "INF", "INFO", "INFO "},
            new[] {"W", "WN", "WRN", "WARN", "WARN "},
            new[] {"E", "ER", "ERR", "EROR", "ERROR"},
            new[] {"F", "FA", "FTL", "FATL", "FATAL"}
        };

        private readonly int width;
        private readonly char caseType;

        public LevelToken([CanBeNull] string format = null)
            : base(WellKnownTokens.Level, format)
        {
            if (format == null || format.Length != 2)
                format = DefaultFormat;

            width = format[1] - '0';
            if (width < 1 || width > 5)
                width = DefaultWidth;

            caseType = format[0];
        }

        public override void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            var index = (int)@event.Level;
            if (index < 0 || index >= UppercaseLevelMap.Length)
                return;

            switch (caseType)
            {
                case 'w':
                    writer.Write(LowercaseLevelMap[index][width - 1]);
                    break;
                case 'u':
                    writer.Write(UppercaseLevelMap[index][width - 1]);
                    break;
                case 't':
                    writer.Write(TitleCaseLevelMap[index][width - 1]);
                    break;
                default:
                    goto case 'u';
            }
        }
    }
}