// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.WebApi.PublicDataSchemas
{
    using System;
    public class SupplierProposalDTO: BaseDTO
    {
        public string BidId { get; set; }

        public string SupplierName { get; set; }

        public string SupplierId { get; set; }

        public DateTime PublishedTime { get; set; }

        public int MinimumUnits { get; set; }

        public double ProposedPrice { get; set; }

        public string Description { get; set; }

    }
}
