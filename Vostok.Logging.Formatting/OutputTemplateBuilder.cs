﻿using System.Collections.Generic;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting
{
    [PublicAPI]
    public class OutputTemplateBuilder
    {
        private readonly List<ITemplateToken> tokens = new List<ITemplateToken>();

        /// <summary>
        /// Creates and returns an <see cref="OutputTemplate"/> assembled from all the parts added earlier.
        /// </summary>
        public OutputTemplate Build() 
            => new OutputTemplate(tokens);

        /// <summary>
        /// Adds a literal text token which will be rendered as-is.
        /// </summary>
        public OutputTemplateBuilder AddText([NotNull] string text)
            => AddToken(new TextToken(text));

        /// <summary>
        /// Adds a token which emits platform-dependent newline.
        /// </summary>
        public OutputTemplateBuilder AddNewline()
            => AddToken(new NewlineToken());

        /// <summary>
        /// Adds a token which emits event <see cref="LogEvent.Timestamp"/> in given <paramref name="format"/>.
        /// </summary>
        public OutputTemplateBuilder AddTimestamp([CanBeNull] string format = null)
            => AddToken(new TimestampToken(format));

        /// <summary>
        /// Adds a token which emits a property with given <paramref name="name"/> from event's <see cref="LogEvent.Properties"/> in given <paramref name="format"/>.
        /// </summary>
        public OutputTemplateBuilder AddProperty([NotNull] string name, [CanBeNull] string format = null)
            => AddToken(new PropertyToken(name, format));

        /// <summary>
        /// Adds a token which emits all of event's <see cref="LogEvent.Properties"/> not mentioned elsewhere in the template.
        /// </summary>
        public OutputTemplateBuilder AddAllProperties()
            => AddToken(new PropertiesToken());

        /// <summary>
        /// Adds a token which emits log message rendered from <see cref="LogEvent.MessageTemplate"/> and <see cref="LogEvent.Properties"/> using <see cref="LogMessageFormatter"/>.
        /// </summary>
        public OutputTemplateBuilder AddMessage()
            => AddToken(new MessageToken());

        /// <summary>
        /// Adds a token which emits <see cref="LogEvent.Level"/> in upper case (such as <c>INFO</c>).
        /// </summary>
        public OutputTemplateBuilder AddLevel()
            => AddToken(new LevelToken());

        /// <summary>
        /// Adds a token which emits <see cref="LogEvent.Exception"/> with message, stack trace and inner exception
        /// </summary>
        public OutputTemplateBuilder AddException()
            => AddToken(new ExceptionToken());

        private OutputTemplateBuilder AddToken(ITemplateToken token)
        {
            tokens.Add(token);
            return this;
        }
    }
}