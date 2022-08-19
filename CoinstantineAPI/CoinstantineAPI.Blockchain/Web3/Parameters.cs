using System.Numerics;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Blockchain.Web3
{
    public class Parameters
    {
        public BlockchainUser Sender { get; set; }
        public BigInteger? Value { get; set; }
    }
}
