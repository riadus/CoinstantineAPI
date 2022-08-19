using System.Collections.Generic;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Users.Unicity.Strategies
{
    public class TelegramConstraintsStrategie : BaseConstraintsStrategie
    {
        public TelegramConstraintsStrategie()
        {
            Constraints = new List<Constraint<IProfileItem>>
            {
                new Constraint<IProfileItem>
                {
                    UniqueKey = UniqueKey.Telegram,
                    ContraintToCheck = user => user.Telegram?.TelegramId,
                    Navigation = user => user.Telegram
                },
                new Constraint<IProfileItem>
                {
                    UniqueKey = UniqueKey.Phonenumber,
                    ContraintToCheck = user => user.Phonenumber,
                    Navigation = null
                }
            };
        }
    }
}
