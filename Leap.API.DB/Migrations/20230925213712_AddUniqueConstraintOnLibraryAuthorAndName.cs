using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Leap.API.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintOnLibraryAuthorAndName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Libraries_Author_Name",
                table: "Libraries",
                columns: new[] { "Author", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Libraries_Author_Name",
                table: "Libraries");
        }
    }
}
