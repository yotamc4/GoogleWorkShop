using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class Seller
    {
        string Id { get; set; }

        string Name { get; set; }

        double Rating { get; set; }

        int ReviewsCounter { get; set; }


    }
}
