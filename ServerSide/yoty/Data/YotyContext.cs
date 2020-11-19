using Microsoft.EntityFrameworkCore;
using Yoty.Data.Entities;

namespace Yoty.Data
{
    public class YotyContext : DbContext
    {
        public DbSet<BuyerEntity> Buyers { get; set; }
        public DbSet<SellerEntity> Sellers { get; set; }
        public DbSet<ProductBidEntity> Bids { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = YotyAppData");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SellerOfferEntity>().HasKey(o => new { o.BidId, o.SellerId });
            modelBuilder.Entity<ParticipancyEntity>().HasKey(p => new { p.BidId, p.BuyerId });
        }
    }
}
