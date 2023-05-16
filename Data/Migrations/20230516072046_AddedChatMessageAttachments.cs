using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WASMChat.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedChatMessageAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AvatarId",
                table: "ChatUsers",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "AvatarId",
                table: "Chats",
                type: "integer",
                nullable: false,
                defaultValue: 2,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "AttachmentId",
                table: "ChatMessages",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_AttachmentId",
                table: "ChatMessages",
                column: "AttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_DatabaseFiles_AttachmentId",
                table: "ChatMessages",
                column: "AttachmentId",
                principalTable: "DatabaseFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_DatabaseFiles_AttachmentId",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_AttachmentId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "ChatMessages");

            migrationBuilder.AlterColumn<int>(
                name: "AvatarId",
                table: "ChatUsers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "AvatarId",
                table: "Chats",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 2);
        }
    }
}
