using System;
using CoinstantineAPI.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace CoinstantineAPI.Database
{
    public class ContextProvider : IContextProvider
    {
        private readonly DbContextOptions<CoinstantineContext> _options;
        private IContext _cachedContext;

        public ContextProvider(DbContextOptions<CoinstantineContext> options)
        {
            _options = options;
        }

        public IContext CoinstantineContext => _cachedContext?.Disposed ?? true ? (_cachedContext = new CoinstantineContext(_options)) : _cachedContext;
    } 
}
