using System;
using System.Data.Entity;
using YOTY.Service.WebApi.PublicDataSchemas;

namespace YotyDataModel
{
    public class YotyContext:DbContext
    {
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<ProductBid> Bids { get; set; }
    }
}
