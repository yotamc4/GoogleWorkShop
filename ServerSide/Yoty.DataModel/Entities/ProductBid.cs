using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Yoty.PublicDataSchemas
{
    // data structure represents product bit with crud
    public class ProductBid
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public string Category { get; set; }

        public double MaxPrice { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        List<Uri> ProductImages {get;set;}

        public string Description { get; set; }

        public int PotenialSuplliersCounter { get; set; }

        public int UnitsCounter { get; set; }


    }
}
