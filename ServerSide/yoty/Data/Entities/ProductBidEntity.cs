using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Yoty.Data.Entities
{
    // data structure represents product bit with crud
    public class ProductBidEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public string Category { get; set; }

        public double MaxPrice { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public Uri ProductImage {get;set;}

        public string Description { get; set; }

        public int PotenialSuplliersCounter { get; set; }

        public int UnitsCounter { get; set; }

        public List<SellerOfferEntity> CurrentOffers { get; set; }

        public List<ParticipancyEntity> CurrentParticipancies { get; set; }
    }
}
