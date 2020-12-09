// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace YOTY.Service.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    // data structure represents product bit with crud
    public class BidEntity
    {
        [Key]
        public string Id { get; set; }

        public ProductEntity Product { get; set; }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public string Category { get; set; }

        public double MaxPrice { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public List<Uri> ProductImages {get;set;}

        public string Description { get; set; }

        public int PotenialSuplliersCounter { get; set; }

        public int UnitsCounter { get; set; }

        public List<SellerOfferEntity> CurrentOffers { get; set; }

        public List<ParticipancyEntity> CurrentParticipancies { get; set; }
    }
}
