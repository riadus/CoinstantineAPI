using CoinstantineAPI.Scan.Interfaces;

namespace CoinstantineAPI.Scan.Dtos
{
    public class CryptoCompareToken : TokenName
    {
        public ApiType ApiType => ApiType.CryptoCompare;
    }
}
