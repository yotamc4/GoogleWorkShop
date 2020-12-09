// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using Microsoft.AspNetCore.Mvc;

    public class BidBuyerJoinRequest
    {
        public string buyerId { get; set; }

        [FromRoute]
        public string bidId { get; set; }

        public int Items { get; set; }
    }
}
