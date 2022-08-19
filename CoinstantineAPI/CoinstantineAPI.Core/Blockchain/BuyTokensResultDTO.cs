using System;
using System.Numerics;
using Nethereum.RPC.Eth.DTOs;

namespace CoinstantineAPI.Core.Blockchain
{
    public class BuyTokensResultDTO
    {
        public BigInteger AmountBought { get; set; }
        public BigInteger Value { get; set; }
        public TransactionReceipt TransactionReceipt { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
