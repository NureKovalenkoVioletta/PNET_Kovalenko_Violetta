using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietaryFitnessProject.Migrations
{
    public partial class AddUserAge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 25);

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Age_Range",
                table: "User",
                sql: "[Age] >= 10 AND [Age] <= 120");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Age_Range",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "User");
        }
    }
}
