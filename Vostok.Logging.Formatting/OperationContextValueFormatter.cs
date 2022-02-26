using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Vostok.Commons.Collections;
using Vostok.Commons.Formatting;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Abstractions.Values;
using Vostok.Logging.Formatting.Helpers;
using Vostok.Logging.Formatting.Tokenizer;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting;

[PublicAPI]
public static class OperationContextValueFormatter
{
    private const int StringBuilderCapacity = 64;
    private const int TemplateCacheCapacity = 1000;

    private static readonly INamedTokenFactory TokenFactory = new PropertyTokensFactory();

    private static readonly RecyclingBoundedCache<string, ITemplateToken[]> TemplateCache =
        new RecyclingBoundedCache<string, ITemplateToken[]>(TemplateCacheCapacity, StringComparer.Ordinal);

    public static string Format(
        [NotNull] LogEvent @event,
        [CanBeNull] object value,
        [CanBeNull] string format = null,
        [CanBeNull] IFormatProvider formatProvider = null)
    {
        if (@event == null)
            throw new ArgumentNullException(nameof(@event));

        var builder = StringBuilderCache.Acquire(StringBuilderCapacity);
        var writer = new StringWriter(builder);

        Format(@event, writer, value, format, formatProvider);

        StringBuilderCache.Release(builder);

        return builder.ToString();
    }

    public static void Format(
        [NotNull] LogEvent @event,
        [NotNull] TextWriter writer,
        [CanBeNull] object value,
        [CanBeNull] string format = null,
        [CanBeNull] IFormatProvider formatProvider = null)
    {
        if (@event == null)
            throw new ArgumentNullException(nameof(@event));
        if (writer == null)
            throw new ArgumentNullException(nameof(writer));

        if (value is OperationContextValue contextValue)
            value = contextValue.ToString();

        if (value is not string template || !TemplateTokenizer.CanContainNamedTokens(template))
        {
            PropertyValueFormatter.Format(writer, value, format, formatProvider);
            return;
        }

        var tokens = TemplateCache.Obtain(template, t => TemplateTokenizer.Tokenize(t, TokenFactory).ToArray());

        PaddingFormatHelper.TryParseFormat(format, out var insertLeadingSpace, out var insertTrailingSpace);

        if (insertLeadingSpace)
            writer.WriteSpace();

        foreach (var token in tokens)
            token.Render(@event, writer, formatProvider);

        if (insertTrailingSpace)
            writer.WriteSpace();
    }
}