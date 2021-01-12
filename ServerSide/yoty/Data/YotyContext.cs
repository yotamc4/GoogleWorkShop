using Microsoft.EntityFrameworkCore;
using YOTY.Service.Data.Entities;

namespace YOTY.Service.Data
{
    public class YotyContext : DbContext
    {
        public DbSet<BuyerEntity> Buyers { get; set; }
        public DbSet<SupplierEntity> Suppliers { get; set; }
        public DbSet<BidEntity> Bids { get; set; }

        private string connectionStringOnConfiguring;

        public YotyContext()
        {
        }

        public YotyContext(DbContextOptions<YotyContext> options)
        : base(options)
        {
        }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionStringOnConfiguring/*
                "Server=tcp:yotydb.database.windows.net,1433;Initial Catalog=UniBuyDB;Persist Security Info=False;User ID=tzachioy@gmail.com@yotydb;Password=Kamaodjan13?;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupplierProposalEntity>().HasKey(o => new { o.BidId, o.SupplierId });
            modelBuilder.Entity<ParticipancyEntity>().HasKey(p => new { p.BidId, p.BuyerId });

            modelBuilder.Entity<SupplierProposalEntity>()
                .HasOne(o => o.Bid)
                .WithMany(b => b.CurrentProposals)
                .HasForeignKey(o => o.BidId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SupplierProposalEntity>()
                .HasOne(o => o.Supplier)
                .WithMany(s => s.CurrentProposals)
                .HasForeignKey(s => s.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ParticipancyEntity>()
                .HasOne(p => p.Bid)
                .WithMany(b => b.CurrentParticipancies)
                .HasForeignKey(p => p.BidId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ParticipancyEntity>()
                .HasOne(p => p.Buyer)
                .WithMany(b => b.CurrentParticipancies)
                .HasForeignKey(p => p.BuyerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
