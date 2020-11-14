using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Yoty.PublicDataSchemas
{
    public class Seller
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public double Rating { get; set; }

        public int ReviewsCounter { get; set; }

        public List<SellerOffer> CurrentOffers { get; set; }


    }
}
