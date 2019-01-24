using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionSystem.Data.Migrations
{
    public partial class AddIsEmailSentFieldInItemsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailSent",
                table: "Items",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailSent",
                table: "Items");
        }
    }
}
