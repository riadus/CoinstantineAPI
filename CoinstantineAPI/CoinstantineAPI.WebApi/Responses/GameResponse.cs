using System.Collections.Generic;

namespace CoinstantineAPI.WebApi.Responses
{
    public class GameResponse
    {
        public int Id { get; set; }
        public AirdropDefinitionResponse AirdropDefinition { get; set; }
        public List<AchievementsResponse> Achievements { get; set; }
    }
}
