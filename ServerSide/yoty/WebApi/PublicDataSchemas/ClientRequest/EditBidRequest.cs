using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class EditBidRequest
    {
        [Required]
        public string BidId { get; set; }

        public string NewName { get; set; }

        public Uri NewProductImage { get; set; }

        public string NewDescription { get; set; }

        public string NewCategory { get; set; }

        public string NewSubCategory { get; set; }
    }
}
