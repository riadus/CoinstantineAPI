namespace CoinstantineAPI.Scan.Dtos
{
    public class PriceDto
    {
        public float Rate { get; set; }
        public float Diff { get; set; }
        public float Diff7d { get; set; }
        public string Ts { get; set; }
        public string MarketCapUsd { get; set; }
        public string AvailableSupply { get; set; }
        public string Volume24h { get; set; }
        public string Currency { get; set; }
    }
}
