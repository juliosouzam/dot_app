using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace core.Migrations
{
    /// <inheritdoc />
    public partial class CommentBelongsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "63a8f7d9-ec5d-411c-b0a5-cfbc7258257f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "7d450023-fe19-4b12-ab79-3766c7b57ca9");

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "comments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { "37bc4f3d-b6d6-41cd-ba76-41ff7151c9a2", null, "Admin", "ADMIN" },
                    { "4fb40090-cf09-48ab-ab66-552d6e74c742", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_comments_user_id",
                table: "comments",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_comments_users_user_id",
                table: "comments",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_comments_users_user_id",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "ix_comments_user_id",
                table: "comments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "37bc4f3d-b6d6-41cd-ba76-41ff7151c9a2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "4fb40090-cf09-48ab-ab66-552d6e74c742");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "comments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { "63a8f7d9-ec5d-411c-b0a5-cfbc7258257f", null, "User", "USER" },
                    { "7d450023-fe19-4b12-ab79-3766c7b57ca9", null, "Admin", "ADMIN" }
                });
        }
    }
}
