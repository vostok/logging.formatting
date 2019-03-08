using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Vostok.Logging.Formatting.Tokens;

// ReSharper disable AssignNullToNotNullAttribute

namespace Vostok.Logging.Formatting.Tokenizer
{
    internal class AllNamedTokensFactory : INamedTokenFactory
    {
        private static readonly Dictionary<string, Func<string, ITemplateToken>> SpecialTokens;

        private readonly HashSet<string> observedOrdinaryProperties = new HashSet<string>();

        static AllNamedTokensFactory()
        {
            SpecialTokens = new Dictionary<string, Func<string, ITemplateToken>>(StringComparer.OrdinalIgnoreCase);

            foreach (var tokenType in SpecialTokensTypesProvider.Get())
            {
                var factoryDelegate = CreateFactoryDelegate(tokenType);
                var tokenName = (factoryDelegate(null) as NamedToken)?.Name;

                SpecialTokens.Add(tokenName, factoryDelegate);
            }
        }

        public ITemplateToken Create(string name, string format)
        {
            if (name == WellKnownTokens.Properties)
                return new PropertiesToken(observedOrdinaryProperties, format);

            if (SpecialTokens.TryGetValue(name, out var tokenFactory))
                return tokenFactory(format);

            observedOrdinaryProperties.Add(name);

            return new PropertyToken(name, format);
        }

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