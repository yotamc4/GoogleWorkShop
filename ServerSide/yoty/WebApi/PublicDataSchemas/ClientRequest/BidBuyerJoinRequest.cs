using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class BidBuyerJoinRequest
    {
        public string buyerId { get; }
        public string productBidId { get; }
        public int Items { get; }
    }
}
