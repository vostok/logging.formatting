using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting.Helpers;
using Vostok.Logging.Formatting.Tokenizer;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting
{
    /// <summary>
    /// <para>Represents a helper used to render log event messages.</para>
    /// <para>See <see cref="Format(LogEvent,TextWriter,IFormatProvider)"/> method for details.</para>
    /// </summary>
    [PublicAPI]
    public static class LogMessageFormatter
    {
        private const int TemplateCacheCapacity = 1000;

        private static readonly INamedTokenFactory TokenFactory = new PropertyTokensFactory();

        private static readonly RecyclingBoundedCache<string, ITemplateToken[]> TemplateCache =
            new RecyclingBoundedCache<string, ITemplateToken[]>(TemplateCacheCapacity);

        /// <inheritdoc cref="Format(LogEvent,TextWriter,IFormatProvider)"/>
        public static string Format(
            [NotNull] LogEvent @event,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var builder = StringBuilderCache.Acquire((@event.MessageTemplate?.Length ?? 4) * 2);
            var writer = new StringWriter(builder);

            Format(@event, writer, formatProvider);

            StringBuilderCache.Release(builder);

            return builder.ToString();
        }

        /// <summary>
        /// <para>Renders given <paramref name="event"/>'s the message using its <see cref="LogEvent.MessageTemplate"/> and <see cref="LogEvent.Properties"/>.</para>
        /// <para>Performs substitution of property placeholders present in message template.</para>
        /// <para>Properties are defined in template with following syntax: '{name:format}'. Format part is optional.</para>
        /// <para>Unlike to <see cref="LogEventFormatter"/>, there are no special predefined properties during message formatting. Only event properties are used.</para>
        /// <para>Example message template: 'Hello, {User}! You have {UnreadCount:D5} messages to read.'</para>
        /// </summary>
        public static void Format(
            [NotNull] LogEvent @event,
            [NotNull] TextWriter writer,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var tokens = TemplateCache.Obtain(@event.MessageTemplate, t => TemplateTokenizer.Tokenize(t, TokenFactory).ToArray());

            foreach (var token in tokens)
            {
                token.Render(@event, writer, formatProvider);
            }
        }
    }
}
