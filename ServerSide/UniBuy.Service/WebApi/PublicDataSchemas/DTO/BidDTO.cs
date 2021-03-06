﻿// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.WebApi.PublicDataSchemas
{
    using System;

    // data structure represents product bit with crud
    public class BidDTO: BaseDTO
    {
        public string Id { get; set; }

        public ProductDTO Product{ get; set; }

        public string OwnerId { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public double MaxPrice { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int PotenialSuplliersCounter { get; set; }

        public int UnitsCounter { get; set; }

    }

    public enum BidStatus
    {
        OpenToRegistration,
        SupplierSelection,
        BuyersPayments,
        CompletedSuccssesfully,
        FailedSupplierNotFound,
        FailedNotReachedItemsNumberGoal,
        ClosedByOwner,
        // failed in payments bacause buyers didn't fullfill their commitment?
    }
}
