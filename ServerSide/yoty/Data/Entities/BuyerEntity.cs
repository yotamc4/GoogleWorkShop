using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Yoty.Data.Entities
{
    public class BuyerEntity
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public FacebookAccountEntity FacebookAccount { get; set; }

        public BuyerAccountDetailsEntity BuyerAccountDeatails { get;set;}

        public List<KeyValuePair<ProductBidEntity, int>> CurrentBidsAndItemsCount { get; set; }

    }
}
