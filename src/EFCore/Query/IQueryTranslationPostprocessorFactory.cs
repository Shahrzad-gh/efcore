// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Query
{
    /// <summary>
    ///     <para>
    ///         A factory for creating <see cref="QueryTranslationPostprocessor" /> instances.
    ///     </para>
    ///     <para>
    ///         The service lifetime is <see cref="ServiceLifetime.Singleton" />. This means a single instance
    ///         is used by many <see cref="DbContext" /> instances. The implementation must be thread-safe.
    ///         This service cannot depend on services registered as <see cref="ServiceLifetime.Scoped" />.
    ///     </para>
    /// </summary>
    public interface IQueryTranslationPostprocessorFactory
    {
        /// <summary>
        ///     Creates a new <see cref="QueryTranslationPostprocessor" /> for given <see cref="QueryCompilationContext" />.
        /// </summary>
        /// <param name="queryCompilationContext"> The query compilation context to use. </param>
        /// <returns> The created visitor. </returns>
        QueryTranslationPostprocessor Create(QueryCompilationContext queryCompilationContext);
    }
}
