using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YOTY.Service.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxPrice = table.Column<double>(type: "float", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PotenialSuplliersCounter = table.Column<int>(type: "int", nullable: false),
                    UnitsCounter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuyerAccountDetailsEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyerAccountDetailsEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacebookAccountEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacebookAccountEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    ReviewsCounter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Buyers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacebookAccountId = table.Column<int>(type: "int", nullable: true),
                    BuyerAccountDetailsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buyers_BuyerAccountDetailsEntity_BuyerAccountDetailsId",
                        column: x => x.BuyerAccountDetailsId,
                        principalTable: "BuyerAccountDetailsEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buyers_FacebookAccountEntity_FacebookAccountId",
                        column: x => x.FacebookAccountId,
                        principalTable: "FacebookAccountEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellerOfferEntity",
                columns: table => new
                {
                    BidId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    BidId1 = table.Column<int>(type: "int", nullable: false),
                    SellerId1 = table.Column<int>(type: "int", nullable: false),
                    PublishedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinimumUnits = table.Column<int>(type: "int", nullable: false),
                    OfferedPrice = table.Column<double>(type: "float", nullable: false),
                    OfferDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerOfferEntity", x => new { x.BidId, x.SellerId });
                    table.ForeignKey(
                        name: "FK_SellerOfferEntity_Bids_BidId1",
                        column: x => x.BidId1,
                        principalTable: "Bids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellerOfferEntity_Sellers_SellerId1",
                        column: x => x.SellerId1,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParticipancyEntity",
                columns: table => new
                {
                    BidId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BuyerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    BidId1 = table.Column<int>(type: "int", nullable: false),
                    NumOfUnits = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipancyEntity", x => new { x.BidId, x.BuyerId });
                    table.ForeignKey(
                        name: "FK_ParticipancyEntity_Bids_BidId1",
                        column: x => x.BidId1,
                        principalTable: "Bids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipancyEntity_Buyers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Buyers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buyers_BuyerAccountDetailsId",
                table: "Buyers",
                column: "BuyerAccountDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Buyers_FacebookAccountId",
                table: "Buyers",
                column: "FacebookAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipancyEntity_BidId1",
                table: "ParticipancyEntity",
                column: "BidId1");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipancyEntity_BuyerId",
                table: "ParticipancyEntity",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerOfferEntity_BidId1",
                table: "SellerOfferEntity",
                column: "BidId1");

            migrationBuilder.CreateIndex(
                name: "IX_SellerOfferEntity_SellerId1",
                table: "SellerOfferEntity",
                column: "SellerId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParticipancyEntity");

            migrationBuilder.DropTable(
                name: "SellerOfferEntity");

            migrationBuilder.DropTable(
                name: "Buyers");

            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "Sellers");

            migrationBuilder.DropTable(
                name: "BuyerAccountDetailsEntity");

            migrationBuilder.DropTable(
                name: "FacebookAccountEntity");
        }
    }
}
