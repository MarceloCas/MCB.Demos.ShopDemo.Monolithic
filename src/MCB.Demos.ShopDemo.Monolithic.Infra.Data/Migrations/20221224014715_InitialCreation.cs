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
                name: "customers");

            migrationBuilder.CreateTable(
                name: "customer",
                schema: "customers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    firstname = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    lastname = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    birthdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    createdby = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lastupdatedby = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    lastupdatedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastsourceplatform = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    registryversion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUSTOMERS_CUSTOMER", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CREATED_AT",
                schema: "customers",
                table: "customer",
                column: "createdat");

            migrationBuilder.CreateIndex(
                name: "IX_CREATED_BY",
                schema: "customers",
                table: "customer",
                column: "createdby");

            migrationBuilder.CreateIndex(
                name: "IX_CREATED_BY_LAST_UPDATED_BY",
                schema: "customers",
                table: "customer",
                columns: new[] { "createdby", "lastupdatedby" });

            migrationBuilder.CreateIndex(
                name: "IX_LAST_SOURCE_PLATFORM",
                schema: "customers",
                table: "customer",
                column: "lastsourceplatform");

            migrationBuilder.CreateIndex(
                name: "IX_LAST_UPDATED_AT",
                schema: "customers",
                table: "customer",
                column: "lastupdatedat");

            migrationBuilder.CreateIndex(
                name: "IX_LAST_UPDATED_BY",
                schema: "customers",
                table: "customer",
                column: "lastupdatedby");

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_ID",
                schema: "customers",
                table: "customer",
                column: "tenantid");

            migrationBuilder.CreateIndex(
                name: "IX_TENANT_ID_ID",
                schema: "customers",
                table: "customer",
                columns: new[] { "tenantid", "id" });

            migrationBuilder.CreateIndex(
                name: "UK_EMAIL",
                schema: "customers",
                table: "customer",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer",
                schema: "customers");
        }
    }
}
