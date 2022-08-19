using System.Collections.Generic;
using System.Threading.Tasks;
using CoinstantineAPI.Data;
using Nethereum.RPC.Eth.DTOs;

namespace CoinstantineAPI.Core.Blockchain
{
    public interface IPresaleService
    {
        Task<int> GetCurrentRate();
        Task<PresaleStatus> GetStatus();
        Task<BuyTokensResult> Buy(BlockchainUser buyer, decimal amountOfETH, string email);
        Task<IEnumerable<BuyTokensResult>> GetPurchases(BlockchainUser user);
        Task<IEnumerable<BuyersDTO>> GetBuyers();
        Task<TransactionReceipt> ClaimBack(BlockchainUser buyer);
        Task<decimal> GetAmountRaised();

        Task<TransactionReceipt> GetReceipt(string transactionHash);
    }
}
