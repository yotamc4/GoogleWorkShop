using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace YOTY.Service.Data.Entities
{
    public class SupplierEntity
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public Uri PaymentLink { get; set; }

        public Uri ProfilePicture { get; set; }

        public string PhoneNumber { get; set; }

        public double Rating { get; set; }

        public int ReviewsCounter { get; set; }

        public List<SupplierProposalEntity> CurrentProposals { get; set; }


    }
}
