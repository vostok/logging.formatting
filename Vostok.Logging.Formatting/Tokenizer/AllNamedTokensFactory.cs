using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Vostok.Logging.Formatting.Tokens;

// ReSharper disable AssignNullToNotNullAttribute

namespace Vostok.Logging.Formatting.Tokenizer
{
    internal class AllNamedTokensFactory : INamedTokenFactory
    {
        private static readonly Dictionary<string, Func<string, ITemplateToken>> SpecialTokens;

        static AllNamedTokensFactory()
        {
            SpecialTokens = new Dictionary<string, Func<string, ITemplateToken>>(StringComparer.OrdinalIgnoreCase);

            foreach (var tokenType in GetSpecialTokenTypes())
            {
                var factoryDelegate = CreateFactoryDelegate(tokenType);
                var tokenName = (factoryDelegate(null) as NamedToken)?.Name;

                SpecialTokens.Add(tokenName, factoryDelegate);
            }
        }

        public ITemplateToken Create(string name, string format) =>
            SpecialTokens.TryGetValue(name, out var tokenFactory)
                ? tokenFactory(format)
                : new PropertyToken(name, format);

        private static IEnumerable<Type> GetSpecialTokenTypes() =>
            typeof(NamedToken)
                .Assembly
                .GetTypes()
                .Where(type => typeof(NamedToken).IsAssignableFrom(type))
                .Where(type => type != typeof(NamedToken))
                .Where(type => type != typeof(PropertyToken))
                .Where(type => !type.IsAbstract);

        private static Func<string, ITemplateToken> CreateFactoryDelegate(Type tokenType)
        {
            var constructor = tokenType.GetConstructor(new[] {typeof(string)});

            var formatExpression = Expression.Parameter(typeof(string));
            var newExpression = Expression.New(constructor, formatExpression);
            var castExpression = Expression.Convert(newExpression, typeof(ITemplateToken));
            var lambdaExpression = Expression.Lambda(castExpression, formatExpression);

            return (Func<string, ITemplateToken>)lambdaExpression.Compile();
        }
    }
}