// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;

    public class EditBidRequest
    {
        [Required]
        [FromRoute]
        string BidId { get; set; }

        string NewName { get; set; }

        List<Uri> NewProductImages { get; set; }

        string NewDescription { get; set; }

        string NewCategory { get; set; }

        string NewSubCategory { get; set; }
    }
}
