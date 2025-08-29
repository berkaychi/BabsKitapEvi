using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BabsKitapEvi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSlugToBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Books");
        }
    }
}
