using Microsoft.EntityFrameworkCore.Migrations;

namespace MarcinMroczek.Sfira.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaggedUsers",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Likes",
                table: "Posts",
                newName: "LikesCount");

            migrationBuilder.AddColumn<int>(
                name: "FavoritesCount",
                table: "Posts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavoritesCount",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "LikesCount",
                table: "Posts",
                newName: "Likes");

            migrationBuilder.AddColumn<string>(
                name: "TaggedUsers",
                table: "Posts",
                nullable: true);
        }
    }
}
