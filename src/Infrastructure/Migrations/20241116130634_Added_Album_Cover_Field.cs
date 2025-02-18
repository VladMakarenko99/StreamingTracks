using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Album_Cover_Field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LengthInSecondsTemp",
                table: "Soundtracks",
                nullable: true);

            // Step 2: Copy data from the old column (interval) to the new column (double precision)
            migrationBuilder.Sql(
                @"UPDATE ""Soundtracks""
          SET ""LengthInSecondsTemp"" = EXTRACT(EPOCH FROM ""LengthInSeconds"");");

            // Step 3: Remove the old column
            migrationBuilder.DropColumn(
                name: "LengthInSeconds",
                table: "Soundtracks");

            // Step 4: Rename the new column to the original column name
            migrationBuilder.RenameColumn(
                name: "LengthInSecondsTemp",
                table: "Soundtracks",
                newName: "LengthInSeconds");

            migrationBuilder.AddColumn<string>(
                name: "AlbumCoverFileName",
                table: "Soundtracks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlbumCoverFileName",
                table: "Soundtracks");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "LengthInSecondsTemp",
                table: "Soundtracks",
                nullable: true);

            // Step 2: Copy data from the old column (double) to the new column (interval)
            migrationBuilder.Sql(
                @"UPDATE ""Soundtracks""
          SET ""LengthInSecondsTemp"" = make_interval(secs => ""LengthInSeconds"");");

            // Step 3: Remove the old column
            migrationBuilder.DropColumn(
                name: "LengthInSeconds",
                table: "Soundtracks");

            // Step 4: Rename the new column to the original column name
            migrationBuilder.RenameColumn(
                name: "LengthInSecondsTemp",
                table: "Soundtracks",
                newName: "LengthInSeconds");
        }
    }
}
