using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HQSOFT.Common.Migrations
{
    /// <inheritdoc />
    public partial class CreateTblShare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommonHQShares",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IDParent = table.Column<string>(type: "text", nullable: true),
                    CanRead = table.Column<bool>(type: "boolean", nullable: false),
                    CanWrite = table.Column<bool>(type: "boolean", nullable: false),
                    CanSubmit = table.Column<bool>(type: "boolean", nullable: false),
                    CanShare = table.Column<bool>(type: "boolean", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonHQShares", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommonHQShareIdentityUser",
                columns: table => new
                {
                    HQShareId = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonHQShareIdentityUser", x => new { x.HQShareId, x.IdentityUserId });
                    table.ForeignKey(
                        name: "FK_CommonHQShareIdentityUser_AbpUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommonHQShareIdentityUser_CommonHQShares_HQShareId",
                        column: x => x.HQShareId,
                        principalTable: "CommonHQShares",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommonHQShareIdentityUser_HQShareId_IdentityUserId",
                table: "CommonHQShareIdentityUser",
                columns: new[] { "HQShareId", "IdentityUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_CommonHQShareIdentityUser_IdentityUserId",
                table: "CommonHQShareIdentityUser",
                column: "IdentityUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommonHQShareIdentityUser");

            migrationBuilder.DropTable(
                name: "CommonHQShares");
        }
    }
}
