using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForeinKeysNullability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatPosts_AspNetUsers_AuthorId",
                table: "ChatPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_PhotoImages_AspNetUsers_AppUserId",
                table: "PhotoImages");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "PhotoImages",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "ChatPosts",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatPosts_AspNetUsers_AuthorId",
                table: "ChatPosts",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoImages_AspNetUsers_AppUserId",
                table: "PhotoImages",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatPosts_AspNetUsers_AuthorId",
                table: "ChatPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_PhotoImages_AspNetUsers_AppUserId",
                table: "PhotoImages");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "PhotoImages",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "ChatPosts",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatPosts_AspNetUsers_AuthorId",
                table: "ChatPosts",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoImages_AspNetUsers_AppUserId",
                table: "PhotoImages",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
