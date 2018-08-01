using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting.Tokenizer;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting
{
    /// <summary>
    /// <para>Represents a pattern used to render <see cref="LogEvent"/>s into text representation.</para>
    /// <para>The syntax allows properties in the following format resembling native .NET string interpolation: '{prop:format}'. Format part is optional.</para>
    /// <para>Properties are supplied from event <see cref="LogEvent.Properties"/>, but there are a number of special predefined ones:</para>
    /// <list type="bullet">
    ///     <item><c>{Timestamp:format}</c> - inserts <see cref="LogEvent.Timestamp"/> formatted with given optional <c>format</c>. Default format is <c>yyyy-MM-dd HH:mm:ss,fff</c>.<para/></item>
    ///     <item><c>{Uptime:format}</c> - inserts a current process uptime measured in milliseconds and formatted with given optional <c>format</c>.<para/></item>
    ///     <item><c>{Level}</c> - inserts <see cref="LogEvent.Level"/> in upper case (such as <c>INFO</c>).<para/></item>
    ///     <item><c>{Message}</c> - inserts log message rendered from <see cref="LogEvent.MessageTemplate"/> and <see cref="LogEvent.Properties"/> using <see cref="LogMessageFormatter"/>.<para/></item>
    ///     <item><c>{NewLine}</c> - inserts a platform-dependent newline.<para/></item>
    ///     <item><c>{Exception}</c> - inserts <see cref="LogEvent.Exception"/> with message, stack trace and inner exception.<para/></item>
    ///     <item><c>{Properties}</c> - inserts all of <see cref="LogEvent.Properties"/> not included elsewhere during event rendering.<para/></item>
    /// </list>
    /// <para>Ordinary named properties are just referenced by their names, such as <c>{MyProp}</c>.</para>
    /// <para>In the event of name collision, special predefined properties take precedence.</para>
    /// <para>Any text between property tokens in curly braces is rendered as-is.</para>
    /// </summary>
    [PublicAPI]
    public class OutputTemplate : IEquatable<OutputTemplate>
    {
        /// <summary>
        /// A template that always produces empty text regardless of <see cref="LogEvent"/> contents.
        /// </summary>
        public static readonly OutputTemplate Empty 
            = new OutputTemplate(new ITemplateToken[] {});

        /// <summary>
        /// A default template with following representation: <c>{Timestamp} {Uptime} {Level} {Prefix}{Message}{NewLine}{Exception}</c>
        /// </summary>
        public static readonly OutputTemplate Default 
            = Parse($"{{{TokenNames.Timestamp}}} {{{TokenNames.Uptime}}} {{{TokenNames.Level}}} {{{TokenNames.Prefix}}}" +
                    $"{{{TokenNames.Message}}}{{{TokenNames.NewLine}}}{{{TokenNames.Exception}}}");

        /// <summary>
        /// Creates a builder which can be used to construct <see cref="OutputTemplate"/> without going through string parsing.
        /// </summary>
        [NotNull]
        public static OutputTemplateBuilder Create()
            => new OutputTemplateBuilder();

        /// <summary>
        /// Parses given template string and returns an <see cref="OutputTemplate"/> corresponding to it.
        /// </summary>
        [NotNull]
        public static OutputTemplate Parse(string input)
            => new OutputTemplate(TemplateTokenizer.Tokenize(input, new AllNamedTokensFactory()).ToArray());

        private readonly string template;

        internal OutputTemplate(IList<ITemplateToken> tokens)
        {
            Tokens = tokens;

            template = string.Concat(tokens);
        }

        internal IList<ITemplateToken> Tokens { get; }

        public override string ToString() => template;

        #region Equality members

        public bool Equals(OutputTemplate other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return string.Equals(template, other.template, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            return Equals((OutputTemplate) obj);
        }

        public override int GetHashCode()
        {
            return template != null ? template.GetHashCode() : 0;
        }

        #endregion
    }
}
