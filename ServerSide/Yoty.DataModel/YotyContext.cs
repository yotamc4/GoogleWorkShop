using System;
using System.Data.Entity;
using Yoty.PublicDataSchemas;

namespace Yoty.DataModel
{
    public class YotyContext : DbContext
    {
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<ProductBid> Bids { get; set; }
    }
}
