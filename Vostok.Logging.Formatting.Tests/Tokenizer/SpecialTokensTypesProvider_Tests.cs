using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Formatting.Tokenizer;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tests.Tokenizer
{
    public class SpecialTokensTypesProvider_Tests
    {
        [Test]
        public void Should_return_all_special_token_types()
        {
            var hardcodedTypes = SpecialTokensTypesProvider.Get();
            
            var allTypes = GetSpecialTokenTypesViaReflection();

            hardcodedTypes.Should().BeEquivalentTo(allTypes);
        }

        private static IEnumerable<Type> GetSpecialTokenTypesViaReflection() =>
            typeof(NamedToken)
                .Assembly
                .GetTypes()
                .Where(type => typeof(NamedToken).IsAssignableFrom(type))
                .Where(type => type != typeof(NamedToken))
                .Where(type => type != typeof(PropertyToken))
                .Where(type => !type.IsAbstract);
    }
}