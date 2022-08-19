using System.Collections.Generic;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Users.Unicity.Strategies
{
    public interface IUnicityConstraintsStrategie
    {
        IEnumerable<Constraint<IProfileItem>> Constraints { get; }
        IEnumerable<ContextConstraints<IProfileItem>> ContextConstraints { get; }
    }
}
