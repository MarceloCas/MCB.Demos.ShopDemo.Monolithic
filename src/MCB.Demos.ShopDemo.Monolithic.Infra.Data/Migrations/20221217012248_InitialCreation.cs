using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MCB.Demos.ShopDemo.Monolithic.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CUSTOMERS");

            migrationBuilder.CreateTable(
                name: "CUSTOMER",
                schema: "CUSTOMERS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    LastName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSourcePlatform = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    RegistryVersion = table.Column<DateTime>(type: "timestamp with time zone", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUSTOMERS_CUSTOMER", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CREATED_AT",
                schema: "CUSTOMERS",
                table: "CUSTOMER",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CREATED_BY",
                schema: "CUSTOMERS",
                table: "CUSTOMER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CREATED_BY_LAST_UPDATED_BY",
                schema: "CUSTOMERS",
                table: "CUSTOMER",
                columns: new[] { "CreatedBy", "LastUpdatedBy" });

            migrationBuilder.CreateIndex(
                name: "IX_LAST_SOURCE_PLATFORM",
                schema: "CUSTOMERS",
                table: "CUSTOMER",
                column: "LastSourcePlatform");

            migrationBuilder.CreateIndex(
                name: "IX_LAST_UPDATED_AT",
                schema: "CUSTOMERS",
                table: "CUSTOMER",
                column: "LastUpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_LAST_UPDATED_BY",
                schema: "CUSTOMERS",
                table: "CUSTOMER",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_ID",
                schema: "CUSTOMERS",
                table: "CUSTOMER",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_ID_ID",
                schema: "CUSTOMERS",
                table: "CUSTOMER",
                columns: new[] { "TenantId", "Id" });

            migrationBuilder.CreateIndex(
                name: "UK_EMAIL",
                schema: "CUSTOMERS",
                table: "CUSTOMER",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CUSTOMER",
                schema: "CUSTOMERS");
        }
    }
}
