using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CoinstantineAPI.Data
{
    public class UserAirdrops : Entity
    {
        public int UserId { get; set; }
        private List<string> _airdropIds;

        [NotMapped]
        public List<string> AirdropIds
        {
            get { return _airdropIds; }
            set { _airdropIds = value; }
        }

        [Required]
        public string AirdropIdsAsString
        {
            get { return String.Join(",", _airdropIds); }
            set { _airdropIds = value.Split(',').ToList(); }
        }
    }
}
