// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using System;
    public class SupplierProposalDTO: BaseDTO
    {
        string BidId { get; set; }

        string SupplierName { get; set; }

        string ProposalId { get; set; }

        string SupplierId { get; set; }

        DateTime PublishedTime { get; set; }

        int MinimumUnits { get; set; }

        double ProposedPrice { get; set; }

        string Description { get; set; }

    }
}
