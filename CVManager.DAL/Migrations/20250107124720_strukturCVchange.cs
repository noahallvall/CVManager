using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class strukturCVchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactInformation",
                table: "CV",
                newName: "Phone");

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicturePath",
                table: "CV",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "CV",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "CV",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "CV",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "CV",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "CV");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "CV");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "CV");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "CV");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "CV",
                newName: "ContactInformation");

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicturePath",
                table: "CV",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
