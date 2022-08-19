using System.Threading.Tasks;

namespace CoinstantineAPI.Core.External
{
    public interface IPriceService
    {
        Task<Price> GetEtherPrice();
    }
}
