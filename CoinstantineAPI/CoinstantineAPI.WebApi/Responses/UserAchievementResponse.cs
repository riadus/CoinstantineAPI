using System;

namespace CoinstantineAPI.WebApi.Responses
{
    public class UserAchievementResponse
    {
        public string AchievementName { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public DateTime AchievementDate { get; set; }
        public bool Achieved { get; set; }
        public int PercentageDone { get; set; }
    }
}
