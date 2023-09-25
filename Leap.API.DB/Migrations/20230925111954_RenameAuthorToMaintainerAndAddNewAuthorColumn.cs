using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leap.API.DB.Migrations
{
    /// <inheritdoc />
    public partial class RenameAuthorToMaintainerAndAddNewAuthorColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_Authors_AuthorId",
                table: "Libraries");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Libraries",
                newName: "MaintainerId");

            migrationBuilder.RenameIndex(
                name: "IX_Libraries_AuthorId",
                table: "Libraries",
                newName: "IX_Libraries_MaintainerId");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Libraries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Libraries_Authors_MaintainerId",
                table: "Libraries",
                column: "MaintainerId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_Authors_MaintainerId",
                table: "Libraries");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Libraries");

            migrationBuilder.RenameColumn(
                name: "MaintainerId",
                table: "Libraries",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Libraries_MaintainerId",
                table: "Libraries",
                newName: "IX_Libraries_AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Libraries_Authors_AuthorId",
                table: "Libraries",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
