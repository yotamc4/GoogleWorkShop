﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YOTY.Service.Data;

namespace YOTY.Service.Migrations
{
    [DbContext(typeof(YotyContext))]
    partial class YotyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("YOTY.Service.Data.Entities.BidEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChosenProposalBidId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ChosenProposalSupplierId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("MaxPrice")
                        .HasColumnType("float");

                    b.Property<string>("OwnerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Phase")
                        .HasColumnType("int");

                    b.Property<int>("PotenialSuplliersCounter")
                        .HasColumnType("int");

                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SubCategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UnitsCounter")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ChosenProposalBidId", "ChosenProposalSupplierId");

                    b.ToTable("Bids");
                });

            modelBuilder.Entity("YOTY.Service.Data.Entities.BuyerEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Buyers");
                });

            modelBuilder.Entity("YOTY.Service.Data.Entities.ParticipancyEntity", b =>
                {
                    b.Property<string>("BidId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BuyerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("HasVoted")
                        .HasColumnType("bit");

                    b.Property<int>("NumOfUnits")
                        .HasColumnType("int");

                    b.HasKey("BidId", "BuyerId");

                    b.HasIndex("BuyerId");

                    b.ToTable("ParticipancyEntity");
                });

            modelBuilder.Entity("YOTY.Service.Data.Entities.ProductEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductEntity");
                });

            modelBuilder.Entity("YOTY.Service.Data.Entities.SupplierEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<int>("ReviewsCounter")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("YOTY.Service.Data.Entities.SupplierProposalEntity", b =>
                {
                    b.Property<string>("BidId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SupplierId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MinimumUnits")
                        .HasColumnType("int");

                    b.Property<double>("ProposedPrice")
                        .HasColumnType("float");

                    b.Property<DateTime>("PublishedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SupplierName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Votes")
                        .HasColumnType("int");

                    b.HasKey("BidId", "SupplierId");

                    b.HasIndex("SupplierId");

                    b.ToTable("SupplierProposalEntity");
                });

            modelBuilder.Entity("YOTY.Service.Data.Entities.BidEntity", b =>
                {
                    b.HasOne("YOTY.Service.Data.Entities.ProductEntity", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("YOTY.Service.Data.Entities.SupplierProposalEntity", "ChosenProposal")
                        .WithMany()
                        .HasForeignKey("ChosenProposalBidId", "ChosenProposalSupplierId");

                    b.Navigation("ChosenProposal");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("YOTY.Service.Data.Entities.ParticipancyEntity", b =>
                {
                    b.HasOne("YOTY.Service.Data.Entities.BidEntity", "Bid")
                        .WithMany("CurrentParticipancies")
                        .HasForeignKey("BidId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YOTY.Service.Data.Entities.BuyerEntity", "Buyer")
                        .WithMany("CurrentParticipancies")
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bid");

                    b.Navigation("Buyer");
                });

            modelBuilder.Entity("YOTY.Service.Data.Entities.SupplierProposalEntity", b =>
                {
                    b.HasOne("YOTY.Service.Data.Entities.BidEntity", "Bid")
                        .WithMany("CurrentProposals")
                        .HasForeignKey("BidId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YOTY.Service.Data.Entities.SupplierEntity", "Supplier")
                        .WithMany("CurrentProposals")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bid");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("YOTY.Service.Data.Entities.BidEntity", b =>
                {
                    b.Navigation("CurrentParticipancies");

                    b.Navigation("CurrentProposals");
                });

            modelBuilder.Entity("YOTY.Service.Data.Entities.BuyerEntity", b =>
                {
                    b.Navigation("CurrentParticipancies");
                });

            modelBuilder.Entity("YOTY.Service.Data.Entities.SupplierEntity", b =>
                {
                    b.Navigation("CurrentProposals");
                });
#pragma warning restore 612, 618
        }
    }
}
