// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductEntity
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public Uri Image { get; set; }

        public string Description { get; set; }
    }
}
