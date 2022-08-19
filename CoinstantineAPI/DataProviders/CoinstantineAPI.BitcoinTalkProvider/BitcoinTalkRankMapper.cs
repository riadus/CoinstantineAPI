using System.Collections.Generic;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.DataProvider.BitcoinTalkProvider
{
    public class BitcoinTalkRankMapper : IMapper<string, BitcoinTalkRank>
    {
        private readonly Dictionary<string, BitcoinTalkRank> _dictionaryMapper;
        public BitcoinTalkRankMapper()
        {
            _dictionaryMapper = new Dictionary<string, BitcoinTalkRank>{
                {"brand new", BitcoinTalkRank.BrandNew},
                {"newbie", BitcoinTalkRank.Newbie},
                {"jr. member", BitcoinTalkRank.JrMember},
                {"member", BitcoinTalkRank.Member},
                {"full member", BitcoinTalkRank.FullMember},
                {"sr. member", BitcoinTalkRank.SrMember},
                {"hero member", BitcoinTalkRank.HeroMember},
                {"legendary", BitcoinTalkRank.Legendary},
            };
        }

        public BitcoinTalkRank Map(string source)
        {
            return _dictionaryMapper[source.ToLower()];
        }
    }
}
