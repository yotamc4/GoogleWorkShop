using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class ProductRequest
    {
        public string Name { get; set; }

        public Uri Image { get; set; }

        public string Description { get; set; }
    }
}
