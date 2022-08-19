using Nethereum.ABI.FunctionEncoding.Attributes;

namespace CoinstantineAPI.Blockchain.DTO.Coinstantine
{
    public class SubscribeEvent
    {
        [Parameter("bytes32", "userId", 1)]
        public byte[] UserId { get; set; }

        [Parameter("bytes32", "airdropId", 2)]
        public byte[] AirdropId { get; set; }
    }
}