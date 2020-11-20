using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    // data structure represents product bit with crud
    public class Bid
    {
        string Id { get; set; }

        string Name { get; set; }

        string OwnerId { get; set; }

        string Category { get; set; }

        string SubCategory { get; set; }

        double MaxPrice { get; set; }

        DateTime CreationDate { get; set; }

        DateTime ExpirationDate { get; set; }

        List<Uri> ProductImages {get;set;}

        string Description { get; set; }

        int PotenialSuplliersCounter { get; set; }

        int UnitsCounter { get; set; }

    }
}
