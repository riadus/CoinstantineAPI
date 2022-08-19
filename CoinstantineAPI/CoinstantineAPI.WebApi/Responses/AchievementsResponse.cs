using System.Collections.Generic;

namespace CoinstantineAPI.WebApi.Responses
{
    public class AchievementsResponse
    {
        public ApiUserResponse ApiUser { get; set; }
        public List<UserAchievementResponse> UserAchievements { get; set; }
    }
}
