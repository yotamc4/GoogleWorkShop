// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using System;

    public class SupplierProposalRequest
    {
        string SupplierId { get; }

        DateTime PublishedTime { get; }

        int MinimumUnits { get; }

        double ProposedPrice { get; }

        string Description { get; }
    }
}