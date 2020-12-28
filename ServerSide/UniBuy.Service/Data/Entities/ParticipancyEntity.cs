// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ParticipancyEntity
    {
        public int NumOfUnits { get; set; }

        public bool HasVoted { get; set; }

        //-----------------------------
        //Relationships
        [Required]
        public BuyerEntity Buyer { get; set; }

        [Required]
        public BidEntity Bid { get; set; }

        public string BidId { get; set; }

        public string BuyerId { get; set; }
    }
}
