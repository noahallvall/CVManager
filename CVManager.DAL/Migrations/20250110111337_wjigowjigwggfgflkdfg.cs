using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class wjigowjigwggfgflkdfg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "ProjectId",
                keyValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "ownerId",
                table: "Project",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ownerId",
                table: "Project");

            migrationBuilder.InsertData(
                table: "Project",
                columns: new[] { "ProjectId", "ProjectDescription", "ProjectName", "UploadDate" },
                values: new object[] { 1, "En databas för Aliens", "MIB", null });
        }
    }
}
