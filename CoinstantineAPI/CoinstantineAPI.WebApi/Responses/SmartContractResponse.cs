namespace CoinstantineAPI.WebApi.Responses
{
    public class SmartContractResponse
    {
        public string Abi { get; set; }
        public string Address { get; set; }
        public TokenResponse Token { get; set; }
        public string Name { get; set; }

        public bool IsCoinstantine => Name == "Coinstantine";
        public bool IsMOCoinstantine => Name == "MOCoinstantine";
        public bool IsPresaleContract => Name == "Presale";
        public bool IsSaleContract => Name == "Sale";
    }
}
