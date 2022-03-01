using System;
using System.Linq;
using Vostok.Commons.Collections;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tokenizer;

internal static class TemplatesCache
{
    private const int Capacity = 1000;

    private static readonly INamedTokenFactory TokenFactory = new PropertyTokensFactory();

    private static readonly RecyclingBoundedCache<string, ITemplateToken[]> TemplateCache =
        new RecyclingBoundedCache<string, ITemplateToken[]>(Capacity, StringComparer.Ordinal);

    public static ITemplateToken[] ObtainTokens(string template) =>
        TemplateCache.Obtain(template, t => TemplateTokenizer.Tokenize(t, TokenFactory).ToArray());
}