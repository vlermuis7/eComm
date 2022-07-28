using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommWeb.Migrations
{
    public partial class AddIdentityUserNameToItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserName",
                table: "Item",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityUserName",
                table: "Item");
        }
    }
}
