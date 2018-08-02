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
    /// <para>Represents a pattern used to render <see cref="T:Vostok.Logging.Abstractions.LogEvent" />s into text representation.</para>
    /// <para>The syntax allows properties in the following format resembling native .NET string interpolation: '{prop:format}'. Format part is optional.</para>
    /// <para>Properties are supplied from event <see cref="P:Vostok.Logging.Abstractions.LogEvent.Properties" />, but there are a number of special predefined ones:</para>
    /// <list type="bullet">
    ///     <item><description><c>{Timestamp:format}</c> — inserts <see cref="P:Vostok.Logging.Abstractions.LogEvent.Timestamp" /> formatted with given optional <c>format</c>. Default format is <c>yyyy-MM-dd HH:mm:ss,fff</c>.<para /></description></item>
    ///     <item><description><c>{Uptime:format}</c> — inserts a current process uptime measured in milliseconds and formatted with given optional <c>format</c>.<para /></description></item>
    ///     <item><description><c>{Level}</c> — inserts <see cref="P:Vostok.Logging.Abstractions.LogEvent.Level" /> in upper case (such as <c>INFO</c>).<para /></description></item>
    ///     <item><description><c>{Message}</c> — inserts log message rendered from <see cref="P:Vostok.Logging.Abstractions.LogEvent.MessageTemplate" /> and <see cref="P:Vostok.Logging.Abstractions.LogEvent.Properties" /> using <see cref="T:Vostok.Logging.Formatting.LogMessageFormatter" />.<para /></description></item>
    ///     <item><description><c>{NewLine}</c> — inserts a platform-dependent newline.<para /></description></item>
    ///     <item><description><c>{Exception}</c> — inserts <see cref="P:Vostok.Logging.Abstractions.LogEvent.Exception" /> with message, stack trace, inner exception and a trailing newline.<para /></description></item>
    ///     <item><description><c>{Properties}</c> — inserts all of event's <see cref="P:Vostok.Logging.Abstractions.LogEvent.Properties" />.<para /></description></item>
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
        public static readonly OutputTemplate Empty = new OutputTemplate(Array.Empty<ITemplateToken>());

        /// <summary>
        /// A default template with following representation: <c>{Timestamp} {Uptime} {Level} {Prefix}{Message}{NewLine}{Exception}</c>
        /// </summary>
        public static readonly OutputTemplate Default
            = Parse(
                $"{{{PropertyNames.Timestamp}}} {{{PropertyNames.Uptime}}} {{{PropertyNames.Level}}} {{{PropertyNames.Prefix}}}" +
                $"{{{PropertyNames.Message}}}{{{PropertyNames.NewLine}}}{{{PropertyNames.Exception}}}");

        /// <summary>
        /// Creates a builder which can be used to construct <see cref="OutputTemplate"/> without going through string parsing.
        /// </summary>
        [NotNull]
        public static OutputTemplateBuilder Create() => new OutputTemplateBuilder();

        /// <summary>
        /// Parses given template string and returns an <see cref="OutputTemplate"/> corresponding to it.
        /// </summary>
        [NotNull]
        public static OutputTemplate Parse(string input)
            => new OutputTemplate(TemplateTokenizer.Tokenize(input, new AllNamedTokensFactory()).ToArray());

        private readonly string template;

        internal OutputTemplate(IReadOnlyList<ITemplateToken> tokens)
        {
            Tokens = tokens;

            template = string.Concat(tokens);
        }

        internal IReadOnlyList<ITemplateToken> Tokens { get; }

        /// <inheritdoc />
        public override string ToString() => template;

        #region Equality members

        /// <inheritdoc />
        public bool Equals(OutputTemplate other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return string.Equals(template, other.template, StringComparison.Ordinal);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is OutputTemplate other)
                return Equals(other);

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode() => template?.GetHashCode() ?? 0;

        #endregion
    }
}