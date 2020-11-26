using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class NewBidRequst
    {
        string Name { get; }
        string OwnerId { get; }
        string Category { get;  }
        string SubCategory { get; }
        double MaxPrice { get; }
        DateTime ExpirationDate { get; set; }
        // TODO: restrict the description size
        string Description { get; set; }
        List<Uri> ProductImages { get; set; }
    }
}
