using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Manufacturer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Style = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QtyOnHand = table.Column<int>(type: "INTEGER", nullable: false),
                    CommissionPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Salespersons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TerminationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Manager = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salespersons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    BeginDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discounts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    SalespersonId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    SalesDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalespersonCommission = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sales_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sales_Salespersons_SalespersonId",
                        column: x => x.SalespersonId,
                        principalTable: "Salespersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "FirstName", "LastName", "Phone", "StartDate" },
                values: new object[,]
                {
                    { 1, "100 Piedmont Park Dr, Atlanta, GA 30309", "David", "Thompson", "678-555-1001", new DateTime(2023, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "250 Virginia Ave, Atlanta, GA 30306", "Sarah", "Williams", "678-555-1002", new DateTime(2023, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "88 Ponce De Leon Ave, Atlanta, GA 30308", "Michael", "Brown", "770-555-1003", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "415 Grant Park Way, Atlanta, GA 30312", "Jessica", "Davis", "770-555-1004", new DateTime(2024, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "60 Inman Park Ln, Atlanta, GA 30307", "Robert", "Garcia", "404-555-1005", new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CommissionPercentage", "Manufacturer", "Name", "PurchasePrice", "QtyOnHand", "SalePrice", "Style" },
                values: new object[,]
                {
                    { 1, 10.00m, "Trek", "Trail Blazer 3000", 1200.00m, 10, 1899.99m, "Mountain" },
                    { 2, 12.00m, "Specialized", "Road Master Elite", 2000.00m, 7, 3199.99m, "Road" },
                    { 3, 8.00m, "Cannondale", "Urban Glide 500", 600.00m, 15, 999.99m, "Hybrid" },
                    { 4, 15.00m, "Giant", "Aero Speed Pro", 3500.00m, 4, 5499.99m, "Road" },
                    { 5, 9.00m, "Surly", "Fat Tire Explorer", 900.00m, 8, 1499.99m, "Fat Bike" }
                });

            migrationBuilder.InsertData(
                table: "Salespersons",
                columns: new[] { "Id", "Address", "FirstName", "LastName", "Manager", "Phone", "StartDate", "TerminationDate" },
                values: new object[,]
                {
                    { 1, "123 Peachtree St, Atlanta, GA 30301", "James", "Carter", "Sarah Mitchell", "404-555-0101", new DateTime(2022, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, "456 Buckhead Ave, Atlanta, GA 30305", "Olivia", "Chen", "Sarah Mitchell", "404-555-0202", new DateTime(2021, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, "789 Midtown Blvd, Atlanta, GA 30308", "Marcus", "Johnson", "Sarah Mitchell", "404-555-0303", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, "321 Decatur Rd, Atlanta, GA 30307", "Elena", "Rodriguez", "Sarah Mitchell", "404-555-0404", new DateTime(2020, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "BeginDate", "DiscountPercentage", "EndDate", "ProductId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.00m, new DateTime(2025, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 15.00m, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 3, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5.00m, new DateTime(2025, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 }
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "Id", "CustomerId", "Price", "ProductId", "SalesDate", "SalespersonCommission", "SalespersonId" },
                values: new object[,]
                {
                    { 1, 1, 1709.99m, 1, new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 171.00m, 1 },
                    { 2, 2, 3199.99m, 2, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 384.00m, 2 },
                    { 3, 3, 849.99m, 3, new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 68.00m, 1 },
                    { 4, 4, 5499.99m, 4, new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 825.00m, 3 },
                    { 5, 5, 1709.99m, 1, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 171.00m, 2 },
                    { 6, 1, 1424.99m, 5, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 128.25m, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_ProductId",
                table: "Discounts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name_Manufacturer",
                table: "Products",
                columns: new[] { "Name", "Manufacturer" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CustomerId",
                table: "Sales",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ProductId",
                table: "Sales",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_SalespersonId",
                table: "Sales",
                column: "SalespersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Salespersons_FirstName_LastName_Phone",
                table: "Salespersons",
                columns: new[] { "FirstName", "LastName", "Phone" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Salespersons");
        }
    }
}
