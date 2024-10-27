using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatetablestore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverPhotoUrl",
                table: "Store",
                type: "text",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Store",
                type: "text",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descriptions",
                table: "Store",
                type: "text",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Store",
                type: "text",
                rowVersion: true,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
