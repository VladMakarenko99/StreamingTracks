using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdedFIleUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AlbumCoverFileName",
                table: "Soundtracks",
                newName: "MusicFileUrl");

            migrationBuilder.AddColumn<string>(
                name: "AlbumCoverUrl",
                table: "Soundtracks",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlbumCoverUrl",
                table: "Soundtracks");

            migrationBuilder.RenameColumn(
                name: "MusicFileUrl",
                table: "Soundtracks",
                newName: "AlbumCoverFileName");
        }
    }
}
