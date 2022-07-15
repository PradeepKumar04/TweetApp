using Microsoft.EntityFrameworkCore.Migrations;

namespace com.tweetapp.api.Migrations
{
    public partial class InitialModel7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "password",
                schema: "Tweet",
                table: "Users",
                type: "NVARCHAR(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                schema: "Tweet",
                table: "Users",
                type: "NVARCHAR(100)",
                maxLength: 90,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR",
                oldMaxLength: 90);

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                schema: "Tweet",
                table: "Users",
                type: "NVARCHAR(100)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                schema: "Tweet",
                table: "Users",
                type: "NVARCHAR(100)",
                maxLength: 90,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR",
                oldMaxLength: 90);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "Tweet",
                table: "Users",
                type: "NVARCHAR(100)",
                maxLength: 90,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR",
                oldMaxLength: 90);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "password",
                schema: "Tweet",
                table: "Users",
                type: "NVARCHAR",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                schema: "Tweet",
                table: "Users",
                type: "NVARCHAR",
                maxLength: 90,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(100)",
                oldMaxLength: 90);

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                schema: "Tweet",
                table: "Users",
                type: "NVARCHAR",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(100)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                schema: "Tweet",
                table: "Users",
                type: "NVARCHAR",
                maxLength: 90,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(100)",
                oldMaxLength: 90);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "Tweet",
                table: "Users",
                type: "NVARCHAR",
                maxLength: 90,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(100)",
                oldMaxLength: 90);
        }
    }
}
