using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StepLang.Libraries.API.DB.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Libraries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LatestVersionId = table.Column<Guid>(type: "uuid", nullable: true),
                    LibraryVersionId = table.Column<Guid>(type: "uuid", nullable: true),
                    LibraryVersionId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libraries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false),
                    LibraryJson = table.Column<string>(type: "text", nullable: false),
                    LibraryId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryVersions_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_LatestVersionId",
                table: "Libraries",
                column: "LatestVersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_LibraryVersionId",
                table: "Libraries",
                column: "LibraryVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_LibraryVersionId1",
                table: "Libraries",
                column: "LibraryVersionId1");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryVersions_LibraryId",
                table: "LibraryVersions",
                column: "LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Libraries_LibraryVersions_LatestVersionId",
                table: "Libraries",
                column: "LatestVersionId",
                principalTable: "LibraryVersions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Libraries_LibraryVersions_LibraryVersionId",
                table: "Libraries",
                column: "LibraryVersionId",
                principalTable: "LibraryVersions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Libraries_LibraryVersions_LibraryVersionId1",
                table: "Libraries",
                column: "LibraryVersionId1",
                principalTable: "LibraryVersions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_LibraryVersions_LatestVersionId",
                table: "Libraries");

            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_LibraryVersions_LibraryVersionId",
                table: "Libraries");

            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_LibraryVersions_LibraryVersionId1",
                table: "Libraries");

            migrationBuilder.DropTable(
                name: "LibraryVersions");

            migrationBuilder.DropTable(
                name: "Libraries");
        }
    }
}
