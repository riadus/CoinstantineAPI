namespace CoinstantineAPI.Data
{
    public class Token : Entity
    {
        public string Address { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int Supply { get; set; }
        public int Decimals { get; set; }
        public string OwnerAddress { get; set; }

        public override bool Equals(object obj)
        {
            var otherToken = obj as Token;
            return otherToken?.Address == Address;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public SmartContract SmartContract { get; set; }
        public Airdrop Airdrop { get; set; }
    }
}
