using Microsoft.EntityFrameworkCore.Migrations;

namespace com.tweetapp.api.Migrations
{
    public partial class InitialModel10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TweetApp");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "Tweet",
                newName: "Users",
                newSchema: "TweetApp");

            migrationBuilder.AlterColumn<int>(
                name: "gender",
                schema: "TweetApp",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(100)",
                oldMaxLength: 5);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Tweet");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "TweetApp",
                newName: "Users",
                newSchema: "Tweet");

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                schema: "Tweet",
                table: "Users",
                type: "NVARCHAR(100)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
