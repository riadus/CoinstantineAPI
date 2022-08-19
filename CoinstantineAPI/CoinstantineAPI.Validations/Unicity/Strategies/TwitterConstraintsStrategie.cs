using System.Collections.Generic;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Users.Unicity.Strategies
{
    public class TwitterConstraintsStrategie : BaseConstraintsStrategie
    {
        public TwitterConstraintsStrategie()
        {
            Constraints = new List<Constraint<IProfileItem>>
            {
                new Constraint<IProfileItem>
                {
                    UniqueKey = UniqueKey.Twitter,
                    ContraintToCheck = user => user.TwitterProfile?.TwitterId,
                    Navigation = user => user.TwitterProfile
                }
            };
        }
    }
}
