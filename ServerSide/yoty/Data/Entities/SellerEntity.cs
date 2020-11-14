using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Yoty.Data.Entities
{
    public class SellerEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public double Rating { get; set; }

        public int ReviewsCounter { get; set; }

        public List<SellerOfferEntity> CurrentOffers { get; set; }


    }
}
