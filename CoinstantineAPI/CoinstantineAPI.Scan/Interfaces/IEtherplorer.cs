using System.Threading.Tasks;
using CoinstantineAPI.Scan.Dtos;

namespace CoinstantineAPI.Scan.Interfaces
{
    public interface IEtherplorer
    {
        Task<EtherplorerResponse> GetInfo(string address);
    }
}