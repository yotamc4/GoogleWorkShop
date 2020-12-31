using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.Data
{
    public enum BidPhase
    {
        Join,
        Vote,
        Payment,
        CancelledSupplierNotFound,
        CancelledNotEnoughBuyersPayed,
        Completed,
    }
}
