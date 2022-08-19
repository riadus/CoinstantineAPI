using System.ComponentModel.DataAnnotations.Schema;

namespace CoinstantineAPI.Data
{
    public class Subscriber : Entity
    {
        [ForeignKey("BlockchainUserId")]
        public BlockchainUser User { get; set; }
        public Airdrop Airdrop { get; set; }
        public bool Validated { get; set; }
        public bool Blocked { get; set; }
        public bool Airdroped { get; set; }
        public bool HasWithdrawn { get; set; }
        public int AmountToReceive { get; set; }
        public byte[] IdentifierBytes { get; set; }
        public string Identifier { get; set; }

        public override bool Equals(object obj)
        {
            return Identifier == (obj as Subscriber)?.Identifier;
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }
    }
}
