using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HQSOFT.Common.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {        
            migrationBuilder.CreateTable(
                name: "CommonHQAssigneds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IDParent = table.Column<Guid>(type: "uuid", nullable: false),
                    Completeby = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_CommonHQAssigneds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommonHQTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    Project = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    ExpectedStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ExpectedEndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ExpectedTime = table.Column<double>(type: "double precision", nullable: false),
                    Process = table.Column<double>(type: "double precision", nullable: false),
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
                    table.PrimaryKey("PK_CommonHQTasks", x => x.Id);
                });
         
            migrationBuilder.CreateTable(
                name: "CommonHQAssignedIdentityUser",
                columns: table => new
                {
                    HQAssignedId = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonHQAssignedIdentityUser", x => new { x.HQAssignedId, x.IdentityUserId });
                    table.ForeignKey(
                        name: "FK_CommonHQAssignedIdentityUser_AbpUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommonHQAssignedIdentityUser_CommonHQAssigneds_HQAssignedId",
                        column: x => x.HQAssignedId,
                        principalTable: "CommonHQAssigneds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommonHQAssignedIdentityUser_HQAssignedId_IdentityUserId",
                table: "CommonHQAssignedIdentityUser",
                columns: new[] { "HQAssignedId", "IdentityUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_CommonHQAssignedIdentityUser_IdentityUserId",
                table: "CommonHQAssignedIdentityUser",
                column: "IdentityUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropTable(
                name: "CommonHQAssignedIdentityUser");

            migrationBuilder.DropTable(
                name: "CommonHQTasks");

          
            migrationBuilder.DropTable(
                name: "CommonHQAssigneds");

        }
    }
}
