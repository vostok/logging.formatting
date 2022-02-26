using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Vostok.Commons.Collections;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Abstractions.Values;
using Vostok.Logging.Formatting.Helpers;
using Vostok.Logging.Formatting.Tokenizer;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting;

internal static class OperationContextValueFormatter
{
    private const int TemplateCacheCapacity = 1000;

    private static readonly INamedTokenFactory TokenFactory = new PropertyTokensFactory();

    private static readonly RecyclingBoundedCache<string, ITemplateToken[]> TemplateCache =
        new RecyclingBoundedCache<string, ITemplateToken[]>(TemplateCacheCapacity, StringComparer.Ordinal);
    
    public static void Format(
        [NotNull] LogEvent @event,
        [NotNull] TextWriter writer,
        [CanBeNull] object value,
        [CanBeNull] string format = null,
        [CanBeNull] IFormatProvider formatProvider = null)
    {
        if (value is not OperationContextValue contextValue || !TemplateTokenizer.CanContainNamedTokens(contextValue.ToString()))
        {
            PropertyValueFormatter.Format(writer, value, format, formatProvider);
            return;
        }

        var tokens = TemplateCache.Obtain(contextValue.ToString(), t => TemplateTokenizer.Tokenize(t, TokenFactory).ToArray());

        PaddingFormatHelper.TryParseFormat(format, out var insertLeadingSpace, out var insertTrailingSpace);
        
        if (insertLeadingSpace)
            writer.WriteSpace();
        
        foreach (var token in tokens)
            token.Render(@event, writer, formatProvider);
        
        if (insertTrailingSpace)
            writer.WriteSpace();
    }
}