using System;
using System.Collections.Generic;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Aidrops.Requirements.Interfaces
{
    public interface IRequirementToLambda
    {
        IEnumerable<Requirement> GetRequirements(IAirdropRequirement requirement);
        bool MeetsAllRequirement(ApiUser user, IEnumerable<IAirdropRequirement> requirements);
    }
}
