using System.Collections.Generic;
using System.Threading.Tasks;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CoinstantineAPI.Core.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task AddOrUpdateAsync<T>(this DbSet<T> dbSet, IEnumerable<T> records)
            where T : Entity
        {
            foreach (var data in records)
            {
                var exists = await dbSet.AsNoTracking().AnyAsync(x => x.Id == data.Id);
                if (exists)
                {
                    dbSet.Update(data);
                    continue;
                }
                await dbSet.AddAsync(data);
            }
        }

        public static Task AddOrUpdateAsync<T>(this DbSet<T> dbSet, T record)
            where T : Entity
        {
            return AddOrUpdateAsync(dbSet, new List<T> { record });
        }
    }
}
