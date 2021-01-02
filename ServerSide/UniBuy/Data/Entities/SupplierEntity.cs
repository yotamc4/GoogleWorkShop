// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SupplierEntity
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public double Rating { get; set; }

        public int ReviewsCounter { get; set; }

        public List<SupplierProposalEntity> CurrentProposals { get; set; }


    }
}
