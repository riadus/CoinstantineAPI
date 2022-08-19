using System;
using System.Collections.Generic;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Users.Unicity.Strategies
{
    public class ProfileConstraintsStrategie : BaseConstraintsStrategie
    {
        public ProfileConstraintsStrategie()
        {
            Constraints = new List<Constraint<IProfileItem>>
            {
                new Constraint<IProfileItem>
                {
                    UniqueKey = UniqueKey.DeviceId,
                    ContraintToCheck = user => user.DeviceId,
                    Navigation = null
                },
                new Constraint<IProfileItem>
                {
                    UniqueKey = UniqueKey.UniqueId,
                    ContraintToCheck = user => user.UniqueId,
                    Navigation = null
                }
            };
        }
    }
}
