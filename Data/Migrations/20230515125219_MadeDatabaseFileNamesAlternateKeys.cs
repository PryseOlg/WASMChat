using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WASMChat.Data.Migrations
{
    /// <inheritdoc />
    public partial class MadeDatabaseFileNamesAlternateKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_DatabaseFiles_Name",
                table: "DatabaseFiles",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_DatabaseFiles_Name",
                table: "DatabaseFiles");
        }
    }
}
