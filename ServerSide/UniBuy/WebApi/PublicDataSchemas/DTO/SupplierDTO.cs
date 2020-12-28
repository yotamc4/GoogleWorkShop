// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.WebApi.PublicDataSchemas
{
    public class SupplierDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Rating { get; set; }

        public string Email { get; set; }

        public int ReviewsCounter { get; set; }
    }
}

