using System;
using System.Collections.Generic;
using Vostok.Logging.Formatting.Tokens;

namespace Vostok.Logging.Formatting.Tokenizer
{
    internal static class SpecialTokensTypesProvider
    {
        /// <summary>
        /// <para>Provides a <see cref="Type"/> objects for types that represent parts of log string.</para>
        /// <para>All of these types is non-abstract types inherited from <see cref="NamedToken"/> except <see cref="PropertyToken"/>.</para>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> Get() =>
            new[]
            {
                // (epeshk): Don't use reflection here to obtain a set of types.
                // We hardcoded this for performance improvement in special case
                // when our assembly is merged into other assembly with large number of types.
                // You can find original reflection-based code in tests.
                typeof(ExceptionToken),
                typeof(LevelToken),
                typeof(OperationContextToken),
                typeof(MessageToken),
                typeof(NewlineToken),
                typeof(PropertiesToken),
                typeof(TimestampToken),
                typeof(UptimeToken)
            };
    }
}