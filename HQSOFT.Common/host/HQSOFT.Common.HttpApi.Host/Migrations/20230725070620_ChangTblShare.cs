using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HQSOFT.Common.Migrations
{
    /// <inheritdoc />
    public partial class ChangTblShare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommonHQShareIdentityUser_CommonHQShares_HQShareId",
                table: "CommonHQShareIdentityUser");

            migrationBuilder.AddColumn<Guid>(
                name: "IdentityUserId",
                table: "CommonHQShares",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommonHQShares_IdentityUserId",
                table: "CommonHQShares",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommonHQShares_AbpUsers_IdentityUserId",
                table: "CommonHQShares",
                column: "IdentityUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommonHQShares_AbpUsers_IdentityUserId",
                table: "CommonHQShares");

            migrationBuilder.DropIndex(
                name: "IX_CommonHQShares_IdentityUserId",
                table: "CommonHQShares");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "CommonHQShares");

            migrationBuilder.AddForeignKey(
                name: "FK_CommonHQShareIdentityUser_CommonHQShares_HQShareId",
                table: "CommonHQShareIdentityUser",
                column: "HQShareId",
                principalTable: "CommonHQShares",
                principalColumn: "Id");
        }
    }
}
