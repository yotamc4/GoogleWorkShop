using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YOTY.Service.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buyers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    ReviewsCounter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParticipancyEntity",
                columns: table => new
                {
                    BidId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BuyerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumOfUnits = table.Column<int>(type: "int", nullable: false),
                    HasVoted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipancyEntity", x => new { x.BidId, x.BuyerId });
                    table.ForeignKey(
                        name: "FK_ParticipancyEntity_Buyers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Buyers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierProposalEntity",
                columns: table => new
                {
                    BidId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SupplierId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PublishedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinimumUnits = table.Column<int>(type: "int", nullable: false),
                    ProposedPrice = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Votes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierProposalEntity", x => new { x.BidId, x.SupplierId });
                    table.ForeignKey(
                        name: "FK_SupplierProposalEntity_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OwnerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxPrice = table.Column<double>(type: "float", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PotenialSuplliersCounter = table.Column<int>(type: "int", nullable: false),
                    UnitsCounter = table.Column<int>(type: "int", nullable: false),
                    ChosenProposalBidId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChosenProposalSupplierId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Phase = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bids_ProductEntity_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bids_SupplierProposalEntity_ChosenProposalBidId_ChosenProposalSupplierId",
                        columns: x => new { x.ChosenProposalBidId, x.ChosenProposalSupplierId },
                        principalTable: "SupplierProposalEntity",
                        principalColumns: new[] { "BidId", "SupplierId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bids_ChosenProposalBidId_ChosenProposalSupplierId",
                table: "Bids",
                columns: new[] { "ChosenProposalBidId", "ChosenProposalSupplierId" });

            migrationBuilder.CreateIndex(
                name: "IX_Bids_ProductId",
                table: "Bids",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipancyEntity_BuyerId",
                table: "ParticipancyEntity",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProposalEntity_SupplierId",
                table: "SupplierProposalEntity",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipancyEntity_Bids_BidId",
                table: "ParticipancyEntity",
                column: "BidId",
                principalTable: "Bids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierProposalEntity_Bids_BidId",
                table: "SupplierProposalEntity",
                column: "BidId",
                principalTable: "Bids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_ProductEntity_ProductId",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_Bids_SupplierProposalEntity_ChosenProposalBidId_ChosenProposalSupplierId",
                table: "Bids");

            migrationBuilder.DropTable(
                name: "ParticipancyEntity");

            migrationBuilder.DropTable(
                name: "Buyers");

            migrationBuilder.DropTable(
                name: "ProductEntity");

            migrationBuilder.DropTable(
                name: "SupplierProposalEntity");

            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
