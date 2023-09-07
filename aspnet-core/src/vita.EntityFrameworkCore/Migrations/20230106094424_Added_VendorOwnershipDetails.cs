using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vita.Migrations
{
    public partial class Added_VendorOwnershipDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendorOwnershipDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UniqueIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorUniqueIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartnerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartnerConstitution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartnerNationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CapitalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CapitalShare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProfitShare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RepresentativeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_VendorOwnershipDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorOwnershipDetails_TenantId",
                table: "VendorOwnershipDetails",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorOwnershipDetails");
        }
    }
}
