using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using momken_backend.Dtos.Myfatoorah;

#nullable disable

namespace momken_backend.Migrations
{
    /// <inheritdoc />
    public partial class inital4D : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    FamilyName = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PhoneNumberVerifed = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyfatoorahTempDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<SubscribeOrderTempData>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyfatoorahTempDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderTotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    orderStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OTPs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Otp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MobileNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTPs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartnerClientRoomMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PartnerClientRoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Massage = table.Column<string>(type: "text", nullable: false),
                    IsRecipientShow = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerClientRoomMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PhoneNumberVerifed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartnerStoreTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerStoreTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartnerClientRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    PartnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerClientRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartnerClientRooms_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartnerClientRooms_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscribePartner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false),
                    paymentGateway = table.Column<string>(type: "text", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: false),
                    MonthCount = table.Column<int>(type: "integer", nullable: false),
                    PartnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceId = table.Column<string>(type: "text", nullable: false),
                    start_from = table.Column<DateOnly>(type: "date", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    amount = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscribePartner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscribePartner_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartnerStores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    City = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FamilyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IDNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ImgNationalID = table.Column<string>(type: "text", nullable: false),
                    DateStartComOrFreeRegister = table.Column<DateOnly>(type: "date", maxLength: 255, nullable: false),
                    DateEndComOrFreeRegister = table.Column<DateOnly>(type: "date", nullable: false),
                    ImgStore = table.Column<string>(type: "text", nullable: false),
                    NumberComOrFreeRegister = table.Column<string>(type: "text", nullable: false),
                    NameComOrFreeRegister = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    EmgComOrFreeRegister = table.Column<string>(type: "text", nullable: false),
                    StoreName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DeliveryType = table.Column<int[]>(type: "integer[]", nullable: false),
                    PartnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerStores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartnerStores_PartnerStoreTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "PartnerStoreTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartnerStores_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartnerStoreSubTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerStoreSubTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartnerStoreSubTypes_PartnerStoreTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "PartnerStoreTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Calories = table.Column<string>(type: "text", nullable: false),
                    Allergens = table.Column<string>(type: "text", nullable: false),
                    MineImg = table.Column<string>(type: "text", nullable: false),
                    MoreImgs = table.Column<string[]>(type: "text[]", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    partnerStoreId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrdersClientId = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_PartnerStores_partnerStoreId",
                        column: x => x.partnerStoreId,
                        principalTable: "PartnerStores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_orders_OrdersClientId",
                        column: x => x.OrdersClientId,
                        principalTable: "orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReviewsOfClient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    clientId = table.Column<Guid>(type: "uuid", nullable: false),
                    partnerStoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewMessage = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EvaluationNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewsOfClient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewsOfClient_Clients_clientId",
                        column: x => x.clientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewsOfClient_PartnerStores_partnerStoreId",
                        column: x => x.partnerStoreId,
                        principalTable: "PartnerStores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartnerClientRoomMessages_PartnerClientRoomId",
                table: "PartnerClientRoomMessages",
                column: "PartnerClientRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerClientRooms_ClientId_PartnerId",
                table: "PartnerClientRooms",
                columns: new[] { "ClientId", "PartnerId" });

            migrationBuilder.CreateIndex(
                name: "IX_PartnerClientRooms_PartnerId",
                table: "PartnerClientRooms",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerStores_PartnerId",
                table: "PartnerStores",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerStores_TypeId",
                table: "PartnerStores",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerStoreSubTypes_TypeId",
                table: "PartnerStoreSubTypes",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrdersClientId",
                table: "Products",
                column: "OrdersClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_partnerStoreId",
                table: "Products",
                column: "partnerStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewsOfClient_clientId",
                table: "ReviewsOfClient",
                column: "clientId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewsOfClient_partnerStoreId",
                table: "ReviewsOfClient",
                column: "partnerStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscribePartner_PartnerId",
                table: "SubscribePartner",
                column: "PartnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyfatoorahTempDatas");

            migrationBuilder.DropTable(
                name: "OTPs");

            migrationBuilder.DropTable(
                name: "PartnerClientRoomMessages");

            migrationBuilder.DropTable(
                name: "PartnerClientRooms");

            migrationBuilder.DropTable(
                name: "PartnerStoreSubTypes");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ReviewsOfClient");

            migrationBuilder.DropTable(
                name: "SubscribePartner");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "PartnerStores");

            migrationBuilder.DropTable(
                name: "PartnerStoreTypes");

            migrationBuilder.DropTable(
                name: "Partners");
        }
    }
}
