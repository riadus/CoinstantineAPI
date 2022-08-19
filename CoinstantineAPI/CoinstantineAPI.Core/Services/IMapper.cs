using System;
namespace CoinstantineAPI.Core.Services
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);
    }
}
