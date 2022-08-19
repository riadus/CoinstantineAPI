using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoinstantineAPI.Data
{
    public class Referral : Entity
    {
        [ForeignKey("ReferralId")]
        public List<ApiUser> Users { get; set; }
        public string Code { get; set; }
        public DateTime CreationDateTime { get; set; }
        [ForeignKey("GodFatherId")]
        public ApiUser GodFather { get; set; }

        [NotMapped]
        public bool FirstGeneration { get; set; }
    }
}
