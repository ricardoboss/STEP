using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leap.API.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddAbilityToHaveMultipleMaintainers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_Authors_MaintainerId",
                table: "Libraries");

            migrationBuilder.DropIndex(
                name: "IX_Libraries_MaintainerId",
                table: "Libraries");

            migrationBuilder.DropColumn(
                name: "MaintainerId",
                table: "Libraries");

            migrationBuilder.CreateTable(
                name: "AuthorLibrary",
                columns: table => new
                {
                    LibrariesId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaintainersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorLibrary", x => new { x.LibrariesId, x.MaintainersId });
                    table.ForeignKey(
                        name: "FK_AuthorLibrary_Authors_MaintainersId",
                        column: x => x.MaintainersId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorLibrary_Libraries_LibrariesId",
                        column: x => x.LibrariesId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorLibrary_MaintainersId",
                table: "AuthorLibrary",
                column: "MaintainersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorLibrary");

            migrationBuilder.AddColumn<Guid>(
                name: "MaintainerId",
                table: "Libraries",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_MaintainerId",
                table: "Libraries",
                column: "MaintainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Libraries_Authors_MaintainerId",
                table: "Libraries",
                column: "MaintainerId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
