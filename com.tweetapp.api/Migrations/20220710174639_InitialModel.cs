using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace com.tweetapp.api.Migrations
{
    public partial class InitialModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Tweet");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Tweet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "NVARCHAR", maxLength: 100, nullable: false),
                    first_name = table.Column<string>(type: "NVARCHAR", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "NVARCHAR", maxLength: 100, nullable: true),
                    gender = table.Column<string>(type: "NVARCHAR", maxLength: 5, nullable: false),
                    dob = table.Column<DateTime>(type: "datetime", nullable: false),
                    password = table.Column<string>(type: "NVARCHAR", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("id", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users",
                schema: "Tweet");
        }
    }
}
