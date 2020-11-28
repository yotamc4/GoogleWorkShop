// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using Microsoft.AspNetCore.Mvc;

    public class BidBuyerJoinRequest
    {
        string buyerId { get; set; }

        [FromRoute]
        string bidId { get; set; }

        public int Items { get; set; }
    }
}
