using System.Collections.Generic;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting
{
    /// <summary>
    /// A builder for <see cref="OutputTemplate"/>.
    /// </summary>
    [PublicAPI]
    public class OutputTemplateBuilder
    {
        private readonly List<ITemplateToken> tokens = new List<ITemplateToken>();

        /// <summary>
        /// Creates and returns an <see cref="OutputTemplate"/> assembled from all the parts added earlier.
        /// </summary>
        public OutputTemplate Build() =>
            new OutputTemplate(tokens);

        /// <summary>
        /// Adds a literal text token which will be rendered as-is.
        /// </summary>
        public OutputTemplateBuilder AddText([NotNull] string text) =>
            AddToken(new TextToken(text));

        /// <summary>
        /// Adds a token which emits platform-dependent newline.
        /// </summary>
        public OutputTemplateBuilder AddNewline() =>
            AddToken(new NewlineToken());

        /// <summary>
        /// Adds a token which emits event <see cref="LogEvent.Timestamp"/> in given <paramref name="format"/>.
        /// </summary>
        public OutputTemplateBuilder AddTimestamp([CanBeNull] string format = null) =>
            AddToken(new TimestampToken(format));

        /// <summary>
        /// Adds a token which emits current process uptime measured in milliseconds and formatted in given <paramref name="format"/>.
        /// </summary>
        public OutputTemplateBuilder AddUptime([CanBeNull] string format = null) =>
            AddToken(new UptimeToken(format));

        /// <summary>
        /// <para>Adds a token which emits a property with given <paramref name="name"/> from event's <see cref="LogEvent.Properties"/> in given <paramref name="format"/>.</para>
        /// <para>You can use a special <c>:W</c> format to add a leading space, <c>:w</c> to add a trailing space or <c>:wW</c> to add both.</para>
        /// </summary>
        public OutputTemplateBuilder AddProperty([NotNull] string name, [CanBeNull] string format = null) =>
            AddToken(new PropertyToken(name, format));

        /// <summary>
        /// Adds a token which emits all of event's <see cref="LogEvent.Properties"/>.
        /// </summary>
        public OutputTemplateBuilder AddAllProperties() =>
            AddToken(new PropertiesToken());

        /// <summary>
        /// Adds a token which emits log message rendered from <see cref="LogEvent.MessageTemplate"/> and <see cref="LogEvent.Properties"/> using <see cref="LogMessageFormatter"/>.
        /// </summary>
        public OutputTemplateBuilder AddMessage() =>
            AddToken(new MessageToken());

        /// <summary>
        /// Adds a token which emits <see cref="LogEvent.Level"/> in upper case (such as <c>INFO</c>).
        /// </summary>
        public OutputTemplateBuilder AddLevel() =>
            AddToken(new LevelToken());

        /// <summary>
        /// <para>Adds a token which emits <see cref="LogEvent.Level"/> with given <paramref name="format"/>.</para>
        /// <para>Use a format such as <c>{Level:u3}</c> or <c>{Level:w3}</c> for three-character upper- or lowercase level names, respectively.</para>
        /// </summary>
        public OutputTemplateBuilder AddLevel(string format) =>
            AddToken(new LevelToken(format));

        /// <summary>
        /// Adds a token which emits <see cref="LogEvent.Exception"/> with message, stack trace, inner exception and a trailing newline.
        /// </summary>
        public OutputTemplateBuilder AddException() =>
            AddToken(new ExceptionToken());

        private OutputTemplateBuilder AddToken(ITemplateToken token)
        {
            tokens.Add(token);
            return this;
        }
    }
}