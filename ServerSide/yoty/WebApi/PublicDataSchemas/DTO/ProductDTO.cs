using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class ProductDto
    {
        public string Name { get; set; }

        public List<Uri> Images { get; set; }

        public string Description { get; set; }
    }
}
