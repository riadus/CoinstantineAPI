namespace CoinstantineAPI.Data
{
    public class BlockchainInfo : Entity
    {
        public float Ether { get; set; }
        public float Coinstantine { get; set; }
        public string Address { get; set; }

        public ApiUser ApiUser { get; set; }
    }
}
