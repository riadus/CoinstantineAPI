using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace CoinstantineAPI.Blockchain.DTO.Presale
{
    [FunctionOutput]
    public class ParticipantDTO
    {
        [Parameter("address", "Address", 1)]
        public string Address { get; set; }

        [Parameter("uint256", "Participation", 2)]
        public BigInteger Participation { get; set; }

        [Parameter("uint256", "Tokens", 3)]
        public BigInteger Tokens { get; set; }

        [Parameter("uint256", "Timestamp", 4)]
        public int Timestamp { get; set; }
    }
}
