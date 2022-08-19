using System;
using System.Threading.Tasks;
using CoinstantineAPI.Core.DataProvider;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.IntegrationTests.Mocks
{
    public class MockBitcoinTalkProvider : IBitcoinTalkPublicProfileProvider
    {
       private readonly IBitcoinTalkFactory _bitcoinTalkFactory;

        public MockBitcoinTalkProvider()
        {
            _bitcoinTalkFactory = new BitcoinTalkProfileFactory();
        }

        public Task<BitcoinTalkProfile> GetUser(int id)
        {
            return Task.FromResult(_bitcoinTalkFactory.GetProfile(id));
        }
    }

    public class StringGenerator : IStringGenerator
    {
        public string GenerateLocation(int id)
        {
            return $"0x1234567890ABCEF{id}";
        }

        public string GenerateUsername(string currentUsername, int id)
        {
            return $"{currentUsername}_{id}";
        }
    }

    public interface IStringGenerator
    {
        string GenerateLocation(int id);
        string GenerateUsername(string currentUsername, int id);
    }

    public class BitcoinTalkProfileFactory : IBitcoinTalkFactory
    {
        private readonly IStringGenerator _locationGenerator;
        public BitcoinTalkProfileFactory()
        {
            _locationGenerator = new StringGenerator();
        }

        public BitcoinTalkProfile GetProfile(int id)
        {
            BitcoinTalkProfile profile;
            if (id < 10)
            {
                profile = new BrandNewBitcoinTalkProfile().Build(id);
            }
            else if (id < 50)
            {
                profile = new NewbieBitcoinTalkProfile().Build(id);
            }
            else if (id < 100)
            {
                profile = new JrMemberBitcoinTalkProfile().Build(id);
            }
            else if (id < 150)
            {
                profile = new MemberBitcoinTalkProfile().Build(id);
            }
            else if (id < 200)
            {
                profile = new SrMemberBitcoinTalkProfile().Build(id);
            }
            else if (id < 250)
            {
                profile = new HeroMemberBitcoinTalkProfile().Build(id);
            }
            else
            {
                profile = new LegendaryBitcoinTalkProfile().Build(id);
            }
            profile.Location = _locationGenerator.GenerateLocation(id);
            profile.Username = _locationGenerator.GenerateUsername(profile.Username, id);
            profile.RegistrationDate = new DateTime(2017, 1, 1);
            return profile;
        }
    }

    public interface IBitcoinTalkFactory
    {
        BitcoinTalkProfile GetProfile(int id);
    }


    public interface IBitcoinTalkProfile
    {
        BitcoinTalkProfile Build(int id);
    }

    public class BrandNewBitcoinTalkProfile : IBitcoinTalkProfile
    {
        public BitcoinTalkProfile Build(int id)
        {
            return new BitcoinTalkProfile
            {
                Position = BitcoinTalkRank.BrandNew,
                Activity = 0,
                BctId = id,
                Posts = 0,
                Username = $"BrandNewUser"
            };
        }
    }

    public class NewbieBitcoinTalkProfile : IBitcoinTalkProfile
    {
        public BitcoinTalkProfile Build(int id)
        {
            return new BitcoinTalkProfile
            {
                Position = BitcoinTalkRank.Newbie,
                Activity = 10,
                BctId = id,
                Posts = 10,
                Username = "NewbieUser"
            };
        }
    }

    public class JrMemberBitcoinTalkProfile : IBitcoinTalkProfile
    {
        public BitcoinTalkProfile Build(int id)
        {
            return new BitcoinTalkProfile
            {
                Position = BitcoinTalkRank.JrMember,
                Activity = 20,
                BctId = id,
                Posts = 20,
                Username = "JrMemberUser"
            };
        }
    }

    public class MemberBitcoinTalkProfile : IBitcoinTalkProfile
    {
        public BitcoinTalkProfile Build(int id)
        {
            return new BitcoinTalkProfile
            {
                Position = BitcoinTalkRank.Member,
                Activity = 0,
                BctId = id,
                Posts = 0,
                Username = "MemberUser"
            };
        }
    }

    public class SrMemberBitcoinTalkProfile : IBitcoinTalkProfile
    {
        public BitcoinTalkProfile Build(int id)
        {
            return new BitcoinTalkProfile
            {
                Position = BitcoinTalkRank.SrMember,
                Activity = 100,
                BctId = id,
                Posts = 100,
                Username = "SrMemberUser"
            };
        }
    }

    public class FullMemberBitcoinTalkProfile : IBitcoinTalkProfile
    {
        public BitcoinTalkProfile Build(int id)
        {
            return new BitcoinTalkProfile
            {
                Position = BitcoinTalkRank.FullMember,
                Activity = 200,
                BctId = id,
                Posts = 200,
                Username = "FullMemberUser"
            };
        }
    }

    public class HeroMemberBitcoinTalkProfile : IBitcoinTalkProfile
    {
        public BitcoinTalkProfile Build(int id)
        {
            return new BitcoinTalkProfile
            {
                Position = BitcoinTalkRank.HeroMember,
                Activity = 500,
                BctId = id,
                Posts = 500,
                Username = "HeroMemberUser"
            };
        }
    }

    public class LegendaryBitcoinTalkProfile : IBitcoinTalkProfile
    {
        public BitcoinTalkProfile Build(int id)
        {
            return new BitcoinTalkProfile
            {
                Position = BitcoinTalkRank.Legendary,
                Activity = 5000,
                BctId = id,
                Posts = 5000,
                Username = "LegendaryUser"
            };
        }
    }
}
