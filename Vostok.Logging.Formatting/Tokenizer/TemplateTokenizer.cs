using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Vostok.Commons.Formatting;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tokenizer
{
    internal static class TemplateTokenizer
    {
        private static readonly char[] AllBraces = {OpeningBrace, ClosingBrace};

        private const char OpeningBrace = '{';
        private const char ClosingBrace = '}';
        private const char Underscore = '_';
        private const char Whitespace = ' ';
        private const char Colon = ':';
        private const char At = '@';

        public static bool CanContainNamedTokens(string template)
            => template.IndexOfAny(AllBraces) >= 0;

        [NotNull]
        public static IEnumerable<ITemplateToken> Tokenize([CanBeNull] string template, [NotNull] INamedTokenFactory namedTokenFactory)
        {
            if (string.IsNullOrEmpty(template))
                yield break;

            var nextIndex = 0;

            while (true)
            {
                // (iloktionov): Consume text until we find a beginning of a named token:
                var text = ConsumeText(template, nextIndex, out nextIndex);
                if (text != null)
                    yield return text;

                if (nextIndex == template.Length)
                    yield break;

                // (iloktionov): Try to consume a named token. If it's incorrect, consume it as text:
                var namedToken = ParseNamedToken(template, nextIndex, out nextIndex, namedTokenFactory);
                if (namedToken != null)
                    yield return namedToken;

                if (nextIndex == template.Length)
                    yield break;
            }
        }

        [CanBeNull]
        private static ITemplateToken ConsumeText(string template, int offset, out int next)
        {
            var beginning = offset;
            var builder = StringBuilderCache.Acquire(Math.Max(4, template.Length - offset));

            do
            {
                var currentCharacter = template[offset];
                if (currentCharacter == OpeningBrace)
                {
                    // (iloktionov): When we encounter an opening brace:
                    // (iloktionov): 1. If the next symbol is also an opening brace, we consume it as text (escaping).
                    // (iloktionov): 2. If the next symbol is something different, we end the text token and try to parse a named token.
                    if (offset + 1 < template.Length && template[offset + 1] == OpeningBrace)
                    {
                        builder.Append(currentCharacter);
                        offset++;
                    }
                    else break;
                }
                else
                {
                    builder.Append(currentCharacter);

                    // (iloktionov): Handle escaping with double braces:
                    if (currentCharacter == ClosingBrace)
                    {
                        if (offset + 1 < template.Length && template[offset + 1] == ClosingBrace)
                        {
                            offset++;
                        }
                    }
                }

                offset++;
            } while (offset < template.Length);

            next = offset;

            StringBuilderCache.Release(builder);

            return CreateTextToken(template, beginning, offset - beginning, builder);
        }

        [CanBeNull]
        private static ITemplateToken ParseNamedToken(string template, int offset, out int next, INamedTokenFactory factory)
        {
            var beginning = offset++;

            // (iloktionov): Just move on until we encounter something that should not be in a named token:
            while (offset < template.Length && IsValidInNamedToken(template[offset]))
                offset++;

            // (iloktionov): If we reached the end of template or didn't stop on a closing brace, there will be no named token:
            if (offset == template.Length || template[offset] != ClosingBrace)
            {
                next = offset;

                return CreateTextToken(template, beginning, offset - beginning);
            }

            next = offset + 1;

            // (iloktionov): Raw content is token with braces included, like '{prop:format}'.
            var rawOffset = beginning;
            var rawLength = next - rawOffset;

            // (iloktionov): Token content is token without braces, like 'prop:format'.
            var tokenOffset = rawOffset + 1;
            var tokenLength = rawLength - 2;

            if (TryParseNamedToken(template, tokenOffset, tokenLength, out var name, out var format))
                return factory.Create(name, format);

            return CreateTextToken(template, rawOffset, rawLength);
        }

        [CanBeNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TextToken CreateTextToken(string template, int offset, int length)
        {
            return length == 0 ? null : new TextToken(template, offset, length);
        }

        [CanBeNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TextToken CreateTextToken(string template, int offset, int length, StringBuilder builder)
        {
            if (length == 0)
                return null;

            if (length == builder.Length)
                return new TextToken(template, offset, length);

            return new TextToken(builder.ToString());
        }

        private static bool TryParseNamedToken(
            string template,
            int offset,
            int length,
            out string name,
            out string format)
        {
            if (length == 0)
            {
                name = format = null;
                return false;
            }

            var formatDelimiter = template.IndexOf(Colon, offset, length);
            if (formatDelimiter < offset)
            {
                name = template.Substring(offset, length);
                format = null;
            }
            else
            {
                name = template.Substring(offset, formatDelimiter - offset);
                format = template.Substring(formatDelimiter + 1, offset + length - formatDelimiter - 1);
            }

            if (string.IsNullOrEmpty(format))
                format = null;

            return IsValidName(name) && IsValidFormat(format);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            for (var i = 0; i < name.Length; i++)
                if (!IsValidInName(name[i]))
                    return false;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidFormat(string format)
        {
            if (format == null)
                return true;

            for (var i = 0; i < format.Length; i++)
                if (!IsValidInFormat(format[i]))
                    return false;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidInNamedToken(char c) =>
            IsValidInName(c) || IsValidInFormat(c) || c == Colon;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidInName(char c) =>
            char.IsLetterOrDigit(c) || c == Underscore || c == At;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidInFormat(char c) =>
            c != ClosingBrace && (char.IsLetterOrDigit(c) || char.IsPunctuation(c) || c == Whitespace);
    }
}