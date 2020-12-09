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
        public string BidId { get; set; }

        public string NewName { get; set; }

        public Uri NewProductImage { get; set; }

        public string NewDescription { get; set; }

        public string NewCategory { get; set; }

        public string NewSubCategory { get; set; }
    }
}
