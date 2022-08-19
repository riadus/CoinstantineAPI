using System;
using System.Numerics;

namespace CoinstantineAPI.Core.Blockchain
{
    public class BuyersDTO
    {
        public string Address { get; set; }
        public decimal AmountInvested { get; set; }
        public BigInteger TokensToReceive { get; set; }
        public DateTime LastPurchageDate { get; set; }
    }
}
