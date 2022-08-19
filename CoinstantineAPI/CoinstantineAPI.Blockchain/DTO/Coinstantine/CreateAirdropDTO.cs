using Nethereum.ABI.FunctionEncoding.Attributes;

namespace CoinstantineAPI.Blockchain.DTO.Coinstantine
{
    public class CreateAirdropEvent
    {
        [Parameter("bytes32", "id", 1)]
        public byte[] Id { get; set; }
    }
}