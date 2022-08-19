using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoinstantineAPI.Data
{
    public class AirdropSubscription : Entity
    {
        public int AirdropDefinitionId { get; set; }

        [NotMapped]
        public AirdropDefinition AirdropDefinition { get; set; }

        [InverseProperty("AirdropSubscription")]
        public ICollection<AirdropSubscriber> Subscribers { get; set; }

        public int Count => Subscribers.Count;
    }
}
