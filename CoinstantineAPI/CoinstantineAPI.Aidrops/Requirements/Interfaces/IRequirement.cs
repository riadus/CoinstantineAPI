using System;

namespace CoinstantineAPI.Aidrops.Requirements.Interfaces
{
    public interface IRequirement
    {
        Func<bool> RequirementApplies { get; set; }
    }

    public interface IRequirement<T> : IRequirement
    {
        Func<T, string> Value { get; set; }
        Func<T, bool> MatchFunc { get; set; }
    }
}
