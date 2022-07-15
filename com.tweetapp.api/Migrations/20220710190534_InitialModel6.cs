using Microsoft.EntityFrameworkCore.Migrations;

namespace com.tweetapp.api.Migrations
{
    public partial class InitialModel6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "id",
                schema: "Tweet",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Tweet",
                table: "Users",
                newName: "id");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                schema: "Tweet",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "Tweet",
                table: "Users",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "Tweet",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "Tweet",
                table: "Users",
                newName: "Id");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                schema: "Tweet",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddPrimaryKey(
                name: "id",
                schema: "Tweet",
                table: "Users",
                column: "Id");
        }
    }
}
