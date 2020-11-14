using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Yoty.PublicDataSchemas
{
    public class Buyer
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public FacebookAccount FacebookAccount { get; set; }

        public BuyerAccountDeatails BuyerAccountDeatails { get;set;}

        public List<Tuple<ProductBid, int>> CurrentBidsAndItemsCount { get; set; }

    }
}
