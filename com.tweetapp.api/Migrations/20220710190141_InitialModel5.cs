using Microsoft.EntityFrameworkCore.Migrations;

namespace com.tweetapp.api.Migrations
{
    public partial class InitialModel5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                schema: "Tweet",
                table: "Users",
                type: "NVARCHAR",
                maxLength: 90,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR",
                oldMaxLength: 90,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                schema: "Tweet",
                table: "Users",
                type: "NVARCHAR",
                maxLength: 90,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR",
                oldMaxLength: 90);
        }
    }
}
