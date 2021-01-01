using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YOTY.Service.Data.Entities
{
    public class BuyerEntity
    {
        [Key]
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public Uri ProfilePicture { get; set; }

        public List<ParticipancyEntity> CurrentParticipancies { get; set; }
    }
}
