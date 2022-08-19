using Nethereum.ABI.FunctionEncoding.Attributes;

namespace CoinstantineAPI.Blockchain.DTO.Coinstantine
{
    public class CloseAirdropEvent
    {
        [Parameter("bytes32", "airdropId", 1)]
        public byte[] AirdropId { get; set; }
    }
}