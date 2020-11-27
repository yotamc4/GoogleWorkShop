using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class NewBidRequst
    {
        public string Name { get; }
        public string OwnerId { get; }
        public string Category { get;  }
        public string SubCategory { get; }
        public double MaxPrice { get; }
        public DateTime ExpirationDate { get; set; }
        // TODO: restrict the description size
        public string Description { get; set; }
        public Uri ProductImage { get; set; }
    }
}
