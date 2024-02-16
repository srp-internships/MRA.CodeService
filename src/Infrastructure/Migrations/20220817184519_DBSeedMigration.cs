using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class DBSeedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "ApiKey",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SecretKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKey", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_ApiKey_Company_CompanyGuid",
                        column: x => x.CompanyGuid,
                        principalTable: "Company",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiKey_CompanyGuid",
                table: "ApiKey",
                column: "CompanyGuid",
                unique: true,
                filter: "[CompanyGuid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKey_SecretKey",
                table: "ApiKey",
                column: "SecretKey");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Host",
                table: "Company",
                column: "Host");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Name",
                table: "Company",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKey");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
