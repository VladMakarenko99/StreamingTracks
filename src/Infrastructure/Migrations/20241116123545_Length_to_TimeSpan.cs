using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Length_to_TimeSpan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "LengthInSecondsTemp",
                table: "Soundtracks",
                nullable: true);

            // Step 2: Copy data from the old column (double) to the new column (interval)
            migrationBuilder.Sql(
                @"UPDATE ""Soundtracks""
          SET ""LengthInSecondsTemp"" = make_interval(secs => ""LengthInSeconds"");"
            );

            // Step 3: Remove the old column
            migrationBuilder.DropColumn(
                name: "LengthInSeconds",
                table: "Soundtracks");

            // Step 4: Rename the new column to match the old column's name
            migrationBuilder.RenameColumn(
                name: "LengthInSecondsTemp",
                table: "Soundtracks",
                newName: "LengthInSeconds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add a temporary column of type double
            migrationBuilder.AddColumn<double>(
                name: "LengthInSecondsTemp",
                table: "Soundtracks",
                nullable: true);

            // Step 2: Copy data from the new column (interval) to the old column (double)
            migrationBuilder.Sql(
                @"UPDATE ""Soundtracks""
          SET ""LengthInSecondsTemp"" = EXTRACT(EPOCH FROM ""LengthInSeconds"");"
            );

            // Step 3: Remove the new column
            migrationBuilder.DropColumn(
                name: "LengthInSeconds",
                table: "Soundtracks");

            // Step 4: Rename the temporary column to match the old column's name
            migrationBuilder.RenameColumn(
                name: "LengthInSecondsTemp",
                table: "Soundtracks",
                newName: "LengthInSeconds");
        }
    }
}
