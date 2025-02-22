// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

// ReSharper disable InconsistentNaming
namespace Microsoft.EntityFrameworkCore
{
    public abstract class LoggingTestBase
    {
        [ConditionalFact]
        public void Logs_context_initialization_default_options()
        {
            Assert.Equal(ExpectedMessage(DefaultOptions), ActualMessage(CreateOptionsBuilder));
        }

        [ConditionalFact]
        public void Logs_context_initialization_no_tracking()
        {
            Assert.Equal(
                ExpectedMessage(DefaultOptions + "NoTracking"),
                ActualMessage(s => CreateOptionsBuilder(s).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)));
        }

        [ConditionalFact]
        public void Logs_context_initialization_sensitive_data_logging()
        {
            Assert.Equal(
                ExpectedMessage(DefaultOptions + "SensitiveDataLoggingEnabled"),
                ActualMessage(s => CreateOptionsBuilder(s).EnableSensitiveDataLogging()));
        }

        protected virtual string ExpectedMessage(string optionsFragment)
            => CoreResources.LogContextInitialized(new TestLogger<TestLoggingDefinitions>()).GenerateMessage(
                ProductInfo.GetVersion(),
                nameof(LoggingContext),
                ProviderName,
                optionsFragment ?? "None").Trim();

        protected abstract DbContextOptionsBuilder CreateOptionsBuilder(IServiceCollection services);

        protected abstract string ProviderName { get; }

        protected virtual string DefaultOptions
            => null;

        protected virtual string ActualMessage(Func<IServiceCollection, DbContextOptionsBuilder> optionsActions)
        {
            var loggerFactory = new ListLoggerFactory();
            var optionsBuilder = optionsActions(new ServiceCollection().AddSingleton<ILoggerFactory>(loggerFactory));

            using (var context = new LoggingContext(optionsBuilder))
            {
                var _ = context.Model;
            }

            return loggerFactory.Log.Single(t => t.Id.Id == CoreEventId.ContextInitialized.Id).Message;
        }

        protected class LoggingContext : DbContext
        {
            public LoggingContext(DbContextOptionsBuilder optionsBuilder)
                : base(optionsBuilder.Options)
            {
            }
        }
    }
}
