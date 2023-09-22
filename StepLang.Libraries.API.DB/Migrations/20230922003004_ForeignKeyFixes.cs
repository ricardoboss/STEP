using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StepLang.Libraries.API.DB.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_LibraryVersions_LibraryVersionId",
                table: "Libraries");

            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_LibraryVersions_LibraryVersionId1",
                table: "Libraries");

            migrationBuilder.RenameColumn(
                name: "LibraryVersionId1",
                table: "Libraries",
                newName: "DependentId");

            migrationBuilder.RenameColumn(
                name: "LibraryVersionId",
                table: "Libraries",
                newName: "DependencyId");

            migrationBuilder.RenameIndex(
                name: "IX_Libraries_LibraryVersionId1",
                table: "Libraries",
                newName: "IX_Libraries_DependentId");

            migrationBuilder.RenameIndex(
                name: "IX_Libraries_LibraryVersionId",
                table: "Libraries",
                newName: "IX_Libraries_DependencyId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_LibraryVersions_DependencyId",
                table: "Libraries");

            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_LibraryVersions_DependentId",
                table: "Libraries");

            migrationBuilder.RenameColumn(
                name: "DependentId",
                table: "Libraries",
                newName: "LibraryVersionId1");

            migrationBuilder.RenameColumn(
                name: "DependencyId",
                table: "Libraries",
                newName: "LibraryVersionId");

            migrationBuilder.RenameIndex(
                name: "IX_Libraries_DependentId",
                table: "Libraries",
                newName: "IX_Libraries_LibraryVersionId1");

            migrationBuilder.RenameIndex(
                name: "IX_Libraries_DependencyId",
                table: "Libraries",
                newName: "IX_Libraries_LibraryVersionId");

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
    }
}
