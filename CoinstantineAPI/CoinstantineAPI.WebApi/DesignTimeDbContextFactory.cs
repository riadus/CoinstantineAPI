using System.IO;
using CoinstantineAPI.Core;
using CoinstantineAPI.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CoinstantineAPI.WebApi
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CoinstantineContext>
    {
        public CoinstantineContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var builder = new DbContextOptionsBuilder<CoinstantineContext>();
            if (Constants.ApiEnvironment == "Localhost")
            {
                builder.UseSqlite("Data Source=CoinstantineDB.db");
            }
            else
            {
                builder.UseSqlServer(Constants.ConnectionDb);
            }

            return new CoinstantineContext(builder.Options);
        }
    }
}
