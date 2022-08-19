using System.Collections.Generic;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Users.Unicity.Strategies
{
    public abstract class BaseConstraintsStrategie : IUnicityConstraintsStrategie
    {
        public IEnumerable<Constraint<IProfileItem>> Constraints { get; protected set; }
        public IEnumerable<ContextConstraints<IProfileItem>> ContextConstraints { get; protected set; }
    }
}
