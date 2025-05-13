using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentRegistrationSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class addnewaddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "StudentInformations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "StudentInformations");
        }
    }
}
