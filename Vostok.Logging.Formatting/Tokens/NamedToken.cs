using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    internal abstract class NamedToken : ITemplateToken
    {
        protected NamedToken([NotNull] string name, [CanBeNull] string format)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Format = format;
        }

        [NotNull]
        public string Name { get; }

        [CanBeNull]
        public string Format { get; }

        public abstract void Render(LogEvent @event, TextWriter writer, IFormatProvider formatProvider);

        public override string ToString() => Format == null
            ? $"{{{Name}}}"
            : $"{{{Name}:{Format}}}";
    }
}