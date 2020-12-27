// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using UniBuy.Service.WebApi.PublicDataSchemas;

    // data structure represents product bit with crud
    public class BidEntity
    {

        [Key]
        public string Id { get; set; }

        public ProductEntity Product { get; set; }

        public string OwnerId { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public double MaxPrice { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int PotenialSuplliersCounter { get; set; }

        public int UnitsCounter { get; set; }

        public List<SupplierProposalEntity> CurrentProposals { get; set; }

        public List<ParticipancyEntity> CurrentParticipancies { get; set; }
    }
}
