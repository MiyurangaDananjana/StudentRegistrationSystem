using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentRegistrationSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class addnewprofilephotoandcreatedatetime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateBy",
                table: "StudentInformations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ProfilePhoto",
                table: "StudentInformations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "StudentInformations");

            migrationBuilder.DropColumn(
                name: "ProfilePhoto",
                table: "StudentInformations");
        }
    }
}
