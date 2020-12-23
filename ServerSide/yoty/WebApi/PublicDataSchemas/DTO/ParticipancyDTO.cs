// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using System;
    public class ParticipancyDTO : BaseDTO
    {
        public string BidId { get; set; }

        public string BuyerName { get; set; }

        public string BuyerId { get; set; }

        public int NumOfUnits { get; set; }

        public bool HasVoted { get; set; }

        public bool HasPaid { get; set; }

    }
}
