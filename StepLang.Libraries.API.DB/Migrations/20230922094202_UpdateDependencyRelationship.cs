using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StepLang.Libraries.API.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDependencyRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_LibraryVersions_DependencyId",
                table: "Libraries");

            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_LibraryVersions_DependentId",
                table: "Libraries");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryVersions_Libraries_LibraryId",
                table: "LibraryVersions");

            migrationBuilder.DropIndex(
                name: "IX_LibraryVersions_LibraryId",
                table: "LibraryVersions");

            migrationBuilder.DropIndex(
                name: "IX_Libraries_DependencyId",
                table: "Libraries");

            migrationBuilder.DropIndex(
                name: "IX_Libraries_DependentId",
                table: "Libraries");

            migrationBuilder.DropColumn(
                name: "LibraryId",
                table: "LibraryVersions");

            migrationBuilder.DropColumn(
                name: "DependencyId",
                table: "Libraries");

            migrationBuilder.DropColumn(
                name: "DependentId",
                table: "Libraries");

            migrationBuilder.CreateTable(
                name: "LibraryVersionDependency",
                columns: table => new
                {
                    VersionId = table.Column<Guid>(type: "uuid", nullable: false),
                    DependencyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryVersionDependency", x => new { x.DependencyId, x.VersionId });
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
                name: "IX_LibraryVersionDependency_VersionId",
                table: "LibraryVersionDependency",
                column: "VersionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryVersionDependency");

            migrationBuilder.AddColumn<Guid>(
                name: "LibraryId",
                table: "LibraryVersions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DependencyId",
                table: "Libraries",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DependentId",
                table: "Libraries",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryVersions_LibraryId",
                table: "LibraryVersions",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_DependencyId",
                table: "Libraries",
                column: "DependencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_DependentId",
                table: "Libraries",
                column: "DependentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Libraries_LibraryVersions_DependencyId",
                table: "Libraries",
                column: "DependencyId",
                principalTable: "LibraryVersions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Libraries_LibraryVersions_DependentId",
                table: "Libraries",
                column: "DependentId",
                principalTable: "LibraryVersions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryVersions_Libraries_LibraryId",
                table: "LibraryVersions",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id");
        }
    }
}
