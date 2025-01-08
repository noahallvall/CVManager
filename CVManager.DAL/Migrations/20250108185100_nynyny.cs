using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class nynyny : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CV",
                keyColumn: "CVId",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CV",
                columns: new[] { "CVId", "ProfilePicturePath", "Summary", "UserId" },
                values: new object[] { 1, null, "Clark är en rolig grabb", "ebca9091-87b4-440f-ae6f-7cc292271074" });
        }
    }
}
