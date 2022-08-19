using System.Threading.Tasks;
using CoinstantineAPI.Scan.Dtos;

namespace CoinstantineAPI.Scan.Interfaces
{
    public interface ICryptoCompare
    {
        Task<CryptoCompareResponse> GetInfos(string tokenName);
        Task<CryptoCompareResponse> GetInfosFromEtherdelta(string tokenName);
        Task<CryptoCompareResponse> GetEtherPrice();
    }
}
