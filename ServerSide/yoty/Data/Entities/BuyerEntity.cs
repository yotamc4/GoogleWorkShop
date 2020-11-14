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

        public BuyerAccountDetailsEntity BuyerAccountDetails { get;set;}

        public List<ProductBidEntity> CurrentBidsAndItemsCount { get; set; }

        public List<ParticipancyEntity> CurrentParticipancies { get; set; }
    }
}
