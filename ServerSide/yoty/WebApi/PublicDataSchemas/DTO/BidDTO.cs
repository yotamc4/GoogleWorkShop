using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    // data structure represents product bit with crud
    public class BidDTO: BaseDTO
    {
        public string Id { get; set; }

        public ProductDto ProductDto { get; set; }

        public string OwnerId { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public double MaxPrice { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int PotenialSuplliersCounter { get; set; }

        public int UnitsCounter { get; set; }

    }

    public enum BidStatus
    {
        OpenToRegistration,
        SupplierSelection,
        BuyersPayments,
        CompletedSuccssesfully,
        FailedSupplierNotFound,
        FailedNotReachedItemsNumberGoal,
        ClosedByOwner,
        // failed in payments bacause buyers didn't fullfill their commitment?
    }
}
