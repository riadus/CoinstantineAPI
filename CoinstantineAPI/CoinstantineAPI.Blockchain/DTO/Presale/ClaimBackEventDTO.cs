using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace CoinstantineAPI.Blockchain.DTO.Presale
{
    [Event("ClaimBack")]
    public class ClaimBackEventDTO
    {
        [Parameter("address", "purchaser", 1, true)]
        public string Purchaser { get; set; }

        [Parameter("uint256", "amount", 2)]
        public BigInteger Amount { get; set; }
    }
}
