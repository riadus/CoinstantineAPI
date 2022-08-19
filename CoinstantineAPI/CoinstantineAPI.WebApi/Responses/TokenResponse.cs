namespace CoinstantineAPI.WebApi.Responses
{
    public class TokenResponse
    {
        public string Address { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int Supply { get; set; }
        public int Decimals { get; set; }
        public string OwnerAddress { get; set; }
    }
}
