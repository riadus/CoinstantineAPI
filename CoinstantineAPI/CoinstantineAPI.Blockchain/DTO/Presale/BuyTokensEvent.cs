using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace CoinstantineAPI.Blockchain.DTO.Presale
{
    [Event("BuyTokens")]
    public class BuyTokensEventDTO
    {
        [Parameter("address", "purchaser", 1, true)]
        public string Purchaser { get; set; }

        [Parameter("address", "beneficiary", 2, true)]
        public string Beneficiary { get; set; }

        [Parameter("uint256", "value", 3)]
        public BigInteger Value { get; set; }

        [Parameter("uint256", "amount", 4)]
        public BigInteger Amount { get; set; }
    }
}
