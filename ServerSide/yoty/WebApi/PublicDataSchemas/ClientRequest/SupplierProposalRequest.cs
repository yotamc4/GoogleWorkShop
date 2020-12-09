// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.WebApi.PublicDataSchemas
{
    using System;

    public class SupplierProposalRequest
    {
        public string BidId { get; }
        public string SupplierId { get; }
        public DateTime PublishedTime { get; }
        public int MinimumUnits { get; }
        public double ProposedPrice { get; }
        public string Description { get; }
    }
}