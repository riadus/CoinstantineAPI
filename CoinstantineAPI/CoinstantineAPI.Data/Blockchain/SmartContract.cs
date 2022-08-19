using System.ComponentModel.DataAnnotations.Schema;

namespace CoinstantineAPI.Data
{
    public class SmartContract : Entity
    {
        public string Abi { get; set; }
        public string Address { get; set; }
        [ForeignKey("TokenId")]
        public Token Token { get; set; }
        public string Name { get; set; }

        public bool IsCoinstantine => Name == "Coinstantine";
        public bool IsMOCoinstantine => Name == "MOCoinstantine";
        public bool IsPresaleContract => Name == "Presale";
        public bool IsSaleContract => Name == "Sale";
    }
}
