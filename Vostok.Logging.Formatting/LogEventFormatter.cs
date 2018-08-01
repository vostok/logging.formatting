using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Formatting.Helpers;

namespace Vostok.Logging.Formatting
{
    /// <summary>
    /// <para>Represents a helper used to render log events into text.</para>
    /// <para>See <see cref="Format(LogEvent,TextWriter,OutputTemplate,IFormatProvider)"/> method for details.</para>
    /// <para>See <see cref="OutputTemplate"/> method for template syntax.</para>
    /// </summary>
    [PublicAPI]
    public static class LogEventFormatter
    {
        /// <inheritdoc cref="Format(LogEvent,TextWriter,OutputTemplate,IFormatProvider)"/>
        public static string Format(
            [NotNull] LogEvent @event,
            [NotNull] OutputTemplate template,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var builder = StringBuilderCache.Acquire(template.ToString().Length + @event.MessageTemplate?.Length ?? 0);
            var writer = new StringWriter(builder);

            Format(@event, writer, template, formatProvider);

            StringBuilderCache.Release(builder);

            return builder.ToString();
        }

        /// <summary>
        /// <para>Renders given <paramref name="event"/> using all of its content.</para>
        /// <para>Performs substitution of property placeholders present in <paramref name="template"/>.</para>
        /// <para>Uses template syntax defined for <see cref="OutputTemplate"/>.</para>
        /// <para>Example output template: <c>{Timestamp} {Level} {Prefix} {Message}{NewLine}{Exception}</c></para>
        /// </summary>
        public static void Format(
            [NotNull] LogEvent @event,
            [NotNull] TextWriter writer,
            [NotNull] OutputTemplate template,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            foreach (var token in template.Tokens)
            {
                token.Render(@event, writer, formatProvider);
            }
        }
    }
}