using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Added_TenantConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TenantConfiguration",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    isPhase1 = table.Column<bool>(type: "bit", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipmentJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalFieldsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalData1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalData2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalData3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantConfiguration", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenantConfiguration_TenantId",
                table: "TenantConfiguration",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantConfiguration");
        }
    }
}
