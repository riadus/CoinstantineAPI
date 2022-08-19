using System;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.DataProvider.BitcoinTalkProvider
{
    public class BitcoinTalkUserMapper : IMapper<BitcoinTalkUserDTO, BitcoinTalkProfile>
    {
        private readonly IMapper<string, BitcoinTalkRank> _rankMapper;
        public BitcoinTalkUserMapper(IMapper<string, BitcoinTalkRank> rankMapper)
        {
            _rankMapper = rankMapper;
        }

        public BitcoinTalkProfile Map(BitcoinTalkUserDTO dto)
        {
            int.TryParse(dto.Activity, out int activity);
            int.TryParse(dto.Posts, out int posts);
            int.TryParse(dto.Age, out int age);
            var registrationDate = Parse(dto.RegistrationDate);
            return new BitcoinTalkProfile
            {
                BctId = dto.BctId,
                Username = dto.Name,
                Posts = activity,
                Activity = posts,
                Position = _rankMapper.Map(dto.Position),
                RegistrationDate = registrationDate,
                Aim = dto.Aim,
                Icq = dto.Icq,
                Msn = dto.Msn,
                Yim = dto.Yim,
                Age = age,
                Location = dto.Location,
                Trust = dto.Trust
            };
        }

        private static DateTime Parse(string stringDateTime)
        {
            stringDateTime = stringDateTime.Replace("Today at", DateTime.Now.ToString("d"));
            return DateTime.Parse(stringDateTime);
        }

    }
}
