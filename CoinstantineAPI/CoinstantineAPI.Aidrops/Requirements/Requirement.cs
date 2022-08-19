using System;
using CoinstantineAPI.Aidrops.Requirements.Interfaces;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Aidrops.Requirements
{
    public class Requirement : IRequirement
    {
        public virtual Func<IProfileItem, bool> MatchFunc { get; set; }
        public Func<bool> RequirementApplies { get; set; }
        public virtual Func<IProfileItem, string> Value { get; set; }
    }

    public class Requirement<T> : Requirement where T : IProfileItem
    {
        public new Func<T, bool> MatchFunc { get; set; }
        public new Func<T, string> Value { get; set; }
    }
}
