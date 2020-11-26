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
        string BidId { get; set; }

        string NewName { get; set; }

        List<Uri> NewProductImages { get; set; }

        string NewDescription { get; set; }

        string NewCategory { get; set; }

        string NewSubCategory { get; set; }
    }
}
