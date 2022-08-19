using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoinstantineAPI.Data
{
    public class Airdrop : Entity
    {
        [ForeignKey("CreatorId")]
        public BlockchainUser Creator { get; set; }
        [ForeignKey("TokenId")]
        public Token Token { get; set; }
        [InverseProperty("Airdrop")]
        public List<Subscriber> Subscribers { get; set; }
        public int Amount { get; set; }
        public int NumberOfUsers { get; set; }
        public string AirdropId { get; set; }
        public byte[] AirdropIdBytes { get; set; }
    }
}
