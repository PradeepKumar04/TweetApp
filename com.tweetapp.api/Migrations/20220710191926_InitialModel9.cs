using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace com.tweetapp.api.Migrations
{
    public partial class InitialModel9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastSeen",
                schema: "Tweet",
                table: "Users",
                newName: "last_seen");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_seen",
                schema: "Tweet",
                table: "Users",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "last_seen",
                schema: "Tweet",
                table: "Users",
                newName: "LastSeen");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastSeen",
                schema: "Tweet",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);
        }
    }
}
