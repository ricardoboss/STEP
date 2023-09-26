using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leap.API.DB.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

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
                });

            migrationBuilder.CreateTable(
                name: "Libraries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LatestVersionId = table.Column<Guid>(type: "uuid", nullable: true)
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
                    LibraryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryVersions_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryVersionDependency",
                columns: table => new
                {
                    VersionId = table.Column<Guid>(type: "uuid", nullable: false),
                    DependencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    VersionRange = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryVersionDependency", x => new { x.VersionId, x.DependencyId });
                    table.ForeignKey(
                        name: "FK_LibraryVersionDependency_Libraries_DependencyId",
                        column: x => x.DependencyId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryVersionDependency_LibraryVersions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "LibraryVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorLibrary_MaintainersId",
                table: "AuthorLibrary",
                column: "MaintainersId");

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_Author_Name",
                table: "Libraries",
                columns: new[] { "Author", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_LatestVersionId",
                table: "Libraries",
                column: "LatestVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryVersionDependency_DependencyId",
                table: "LibraryVersionDependency",
                column: "DependencyId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryVersions_LibraryId",
                table: "LibraryVersions",
                column: "LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorLibrary_Libraries_LibrariesId",
                table: "AuthorLibrary",
                column: "LibrariesId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Libraries_LibraryVersions_LatestVersionId",
                table: "Libraries",
                column: "LatestVersionId",
                principalTable: "LibraryVersions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryVersions_Libraries_LibraryId",
                table: "LibraryVersions");

            migrationBuilder.DropTable(
                name: "AuthorLibrary");

            migrationBuilder.DropTable(
                name: "LibraryVersionDependency");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Libraries");

            migrationBuilder.DropTable(
                name: "LibraryVersions");
        }
    }
}
