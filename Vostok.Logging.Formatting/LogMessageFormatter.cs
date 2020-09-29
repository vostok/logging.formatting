using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Vostok.Commons.Collections;
using Vostok.Commons.Formatting;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting.Tokenizer;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting
{
    /// <summary>
    /// <para>Represents a helper used to render log event messages into text.</para>
    /// <para>See <see cref="Format(LogEvent,TextWriter,IFormatProvider)"/> method for details.</para>
    /// <para>See <see cref="OutputTemplate"/> method for template syntax.</para>
    /// </summary>
    [PublicAPI]
    public static class LogMessageFormatter
    {
        private const int TemplateCacheCapacity = 1000;

        private static readonly INamedTokenFactory TokenFactory = new PropertyTokensFactory();

        private static readonly RecyclingBoundedCache<string, ITemplateToken[]> TemplateCache =
            new RecyclingBoundedCache<string, ITemplateToken[]>(TemplateCacheCapacity, StringComparer.Ordinal);

        /// <inheritdoc cref="Format(LogEvent,TextWriter,IFormatProvider)"/>
        public static string Format(
            [NotNull] LogEvent @event,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            if (@event.MessageTemplate == null)
                return string.Empty;

            if (!TemplateTokenizer.CanContainNamedTokens(@event.MessageTemplate))
                return @event.MessageTemplate;

            var builder = StringBuilderCache.Acquire(@event.MessageTemplate.Length * 2);
            var writer = new StringWriter(builder);

            FormatInternal(@event, writer, formatProvider);

            StringBuilderCache.Release(builder);

            return builder.ToString();
        }

        /// <summary>
        /// <para>Renders given <paramref name="event"/>'s message using its <see cref="LogEvent.MessageTemplate"/> and <see cref="LogEvent.Properties"/>.</para>
        /// <para>Performs substitution of property placeholders present in message template.</para>
        /// <para>Uses template syntax defined for <see cref="OutputTemplate"/> (with one key difference outlined below).</para>
        /// <para>Unlike to <see cref="LogEventFormatter"/>, there are no special predefined properties during message formatting. Only event properties are used.</para>
        /// <para>Example message template: <c>'Hello, {User}! You have {UnreadCount:D5} messages to read.'</c></para>
        /// </summary>
        public static void Format(
            [NotNull] LogEvent @event,
            [NotNull] TextWriter writer,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            if (@event.MessageTemplate == null)
                return;

            if (!TemplateTokenizer.CanContainNamedTokens(@event.MessageTemplate))
            {
                if (@event.MessageTemplate.Length > 0)
                    writer.Write(@event.MessageTemplate);
            }
            else FormatInternal(@event, writer, formatProvider);
        }

        private static void FormatInternal(LogEvent @event, TextWriter writer, IFormatProvider formatProvider)
        {
            var tokens = TemplateCache.Obtain(@event.MessageTemplate, t => TemplateTokenizer.Tokenize(t, TokenFactory).ToArray());

            foreach (var token in tokens)
                token.Render(@event, writer, formatProvider);
        }
    }
}