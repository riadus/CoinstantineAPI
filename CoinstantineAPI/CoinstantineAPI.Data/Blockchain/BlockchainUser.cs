using System.Collections.Generic;

namespace CoinstantineAPI.Data
{
    public class BlockchainUser : Entity
    {
        public string Address { get; set; }
        public string PassPhrase { get; set; }
        public string Username { get; set; }
        public string Json { get; set; }

        public ICollection<Airdrop> Airdrops { get; set; }
    }
}
