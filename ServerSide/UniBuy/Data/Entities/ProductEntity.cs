// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ProductEntity
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public Uri Image { get; set; }

        public string Description { get; set; }
    }
}
