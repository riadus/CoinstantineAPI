namespace CoinstantineAPI.Scan.Dtos
{
    public class TokenDto
    {
        public TokenInfoDto TokenInfo { get; set; }
        public float Balance { get; set; }
        public int TotalIn { get; set; }
        public int TotalOut { get; set; }
        public override string ToString()
        {
            return string.Format($"[TokenDto: TokenInfo={TokenInfo}, Balance={Balance}, TotalIn={TotalIn}, TotalOut={TotalOut}]");
        }
    }
}