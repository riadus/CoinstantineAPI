using System.Collections.Generic;

namespace CoinstantineAPI.Scan.Dtos
{
    public class CryptoCompareResponse
    {
        public decimal Btc { get; set; }
        public decimal Usd { get; set; }
        public decimal Eur { get; set; }
        public decimal Eth { get; set; }

        public string Response { get; set; }
        public string Message { get; set; }
        public int Type { get; set; }
        public bool Aggregated { get; set; }
        public List<object> Data { get; set; }

        public bool Success => string.IsNullOrEmpty(Response);
    }
}
