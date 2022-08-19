using Nethereum.ABI.FunctionEncoding.Attributes;

namespace CoinstantineAPI.Blockchain.DTO.Coinstantine
{
    public class DepositEvent
    {
        [Parameter("bytes32", "id", 1)]
        public byte[] Id { get; set; }

        [Parameter("int", "amount", 2)]
        public int Amount { get; set; }
    }
}