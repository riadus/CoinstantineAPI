using System;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Data;
using CoinstantineAPI.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CoinstantineAPI.UnitTests
{
    public class ContextProviderBuilder
    {
        public IContextProvider Build()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<CoinstantineContext>()
                .UseSqlite(connection)
                .EnableSensitiveDataLogging()
                .Options;
            
            return new UnitTestsContextProvider(options);
        }
    }

    public class UnitTestsContextProvider : IContextProvider
    {
        private readonly DbContextOptions<CoinstantineContext> _options;
        private IContext _cachedContext;

        public UnitTestsContextProvider(DbContextOptions<CoinstantineContext> options)
        {
            _options = options;
        }

        public IContext CoinstantineContext => _cachedContext?.Disposed ?? true ? (_cachedContext = new CoinstantineContext(_options, false)) : _cachedContext;
    }
}
