using System;
using System.IO;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Formatting.Tokens
{
    /// <summary>
    /// <see cref="ITemplateToken"/> is a building block of <see cref="OutputTemplate"/>s. Each token is responsible for rendering a specific portion of <see cref="LogEvent"/>.
    /// </summary>
    [PublicAPI]
    public interface ITemplateToken
    {
        void Render([NotNull] LogEvent @event, [NotNull] TextWriter writer, [CanBeNull] IFormatProvider formatProvider);
    }
}