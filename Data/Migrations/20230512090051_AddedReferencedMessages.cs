using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WASMChat.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedReferencedMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReferencedMessageId",
                table: "ChatMessages",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ReferencedMessageId",
                table: "ChatMessages",
                column: "ReferencedMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_ChatMessages_ReferencedMessageId",
                table: "ChatMessages",
                column: "ReferencedMessageId",
                principalTable: "ChatMessages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_ChatMessages_ReferencedMessageId",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_ReferencedMessageId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "ReferencedMessageId",
                table: "ChatMessages");
        }
    }
}
