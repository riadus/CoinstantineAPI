using System.Collections.Generic;

namespace CoinstantineAPI.Scan.Dtos
{
    public class EtherplorerResponse
    {
        public string Address { get; set; }
        public EthDto Eth { get; set; }
        public int CountTxs { get; set; }
        public List<TokenDto> Tokens { get; set; }
    }
}
