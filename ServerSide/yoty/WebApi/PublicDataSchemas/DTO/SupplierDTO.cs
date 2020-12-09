﻿// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    public class SupplierDTO
    {
        string Id { get; set; }

        string Name { get; set; }

        double Rating { get; set; }

        int ReviewsCounter { get; set; }
    }
}

