using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class DealDTO
    {
        public string DealId { get; set; }

        public string CorrolatedBidId { get; set; }

        public ProductDto ProductDTO { get; set; }

        public DealStatus DealStatus { get; set; }

        public SupplierDTO SupplierDTO { get; set; }

        public List<BuyerDTO> Buyers { get; set; } // or should it be list of buyers ids ? or maybe it should not appear here ?  

        public int Items { get; set; }
    }

    public enum DealStatus
    {
        InDeliveryProcess,
        CompletedSuccesFully,
    }
}
