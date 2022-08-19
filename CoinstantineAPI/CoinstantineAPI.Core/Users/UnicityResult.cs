using System.Collections.Generic;

namespace CoinstantineAPI.Core.Users
{
    public class UnicityResult
    {
        public static UnicityResult True => new UnicityResult { AllGood = true, FailedConstaints = new List<UniqueKey>() };
        public bool AllGood { get; set; }
        public IEnumerable<UniqueKey> FailedConstaints { get; set; }
    }
}
