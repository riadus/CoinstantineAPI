using System.Collections.Generic;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Users.Unicity.Strategies
{
    public class BitcoinTalkConstraintsStrategie : BaseConstraintsStrategie
    {
        public BitcoinTalkConstraintsStrategie()
        {
            Constraints = new List<Constraint<IProfileItem>>
            {
                new Constraint<IProfileItem>
                {
                    UniqueKey = UniqueKey.BctId,
                    ContraintToCheck = user => user.BctProfile?.BctId,
                    Navigation = user => user.BctProfile
                },
                new Constraint<IProfileItem>
                {
                    UniqueKey = UniqueKey.BctName,
                    ContraintToCheck = user => user.BctProfile?.Username,
                    Navigation = user => user.BctProfile
                },
                new Constraint<IProfileItem>
                {
                    UniqueKey = UniqueKey.BctLocation,
                    ContraintToCheck = user => user.BctProfile?.Location,
                    Navigation = user => user.BctProfile
                }
            };
        }
    }
}
