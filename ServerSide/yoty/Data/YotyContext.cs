using Microsoft.EntityFrameworkCore;
using YOTY.Service.Data.Entities;

namespace YOTY.Service.Data
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

            modelBuilder.Entity<SellerOfferEntity>()
                .HasOne(o => o.Bid)
                .WithMany(b => b.CurrentOffers)
                .HasForeignKey(o => o.BidId);
            modelBuilder.Entity<SellerOfferEntity>()
                .HasOne(o => o.Seller)
                .WithMany(s => s.CurrentOffers)
                .HasForeignKey(s => s.SellerId);

            modelBuilder.Entity<ParticipancyEntity>()
                .HasOne(p => p.Bid)
                .WithMany(b => b.CurrentParticipancies)
                .HasForeignKey(p => p.BidId);
            modelBuilder.Entity<ParticipancyEntity>()
                .HasOne(p => p.Buyer)
                .WithMany(b => b.CurrentParticipancies)
                .HasForeignKey(p => p.BuyerId);
        }
    }
}
