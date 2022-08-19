using Newtonsoft.Json;

namespace CoinstantineAPI.Scan.Dtos
{
    public class TokenInfoDto
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public int Decimals { get; set; }
        public string Symbol { get; set; }
        public string TotalSupply { get; set; }
        public string Owner { get; set; }
        public int LastUpdated { get; set; }
        public int IssuancesCount { get; set; }
        public int HoldersCount { get; set; }
        [JsonProperty("price")]
        public object PriceInfo { get; set; }
        public double? TotalIn { get; set; }
        public double? TotalOut { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public PriceDto Price => PriceInfo as PriceDto;
        public bool HasValidPrice => !(PriceInfo is bool);

        public override string ToString()
        {
            return string.Format($"[TokenInfoDto: Address={Address}, Name={Name}, Symbol={Symbol}");
        }
    }
}