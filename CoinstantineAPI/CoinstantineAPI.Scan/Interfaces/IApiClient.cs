using System.Threading.Tasks;

namespace CoinstantineAPI.Scan.Interfaces
{
    public interface IApiClient
    {
        Task<T> GetAsync<T>(string url);
    }
}
