// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class BuyerEntity
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        // public FacebookAccountEntity FacebookAccount { get; set; }

        // public BuyerAccountDetailsEntity BuyerAccountDetails { get;set;}

        public List<ParticipancyEntity> CurrentParticipancies { get; set; }
    }
}
