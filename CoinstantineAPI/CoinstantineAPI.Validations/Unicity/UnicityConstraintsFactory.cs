using System;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Users.Unicity.Strategies;

namespace CoinstantineAPI.Users.Unicity
{
    public class UnicityConstraintsFactory : IUnicityConstraintsFactory
    {
        public IUnicityConstraintsStrategie GetStrategie(UnicityTopic topic)
        {
            switch (topic)
            {
                case UnicityTopic.BitcoinTalk:
                    return new BitcoinTalkConstraintsStrategie();
                case UnicityTopic.Telegram:
                    return new TelegramConstraintsStrategie();
                case UnicityTopic.Twitter:
                    return new TwitterConstraintsStrategie();
                case UnicityTopic.Profile:
                    return new ProfileConstraintsStrategie();
            }
            throw new NotImplementedException();
        }
    }
}
