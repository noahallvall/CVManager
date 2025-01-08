using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class nymigrering1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CV_AspNetUsers_UserId1",
                table: "CV");

            migrationBuilder.DropIndex(
                name: "IX_CV_UserId1",
                table: "CV");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "CV");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CV",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CV_UserId",
                table: "CV",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CV_AspNetUsers_UserId",
                table: "CV",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CV_AspNetUsers_UserId",
                table: "CV");

            migrationBuilder.DropIndex(
                name: "IX_CV_UserId",
                table: "CV");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CV",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "CV",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CV_UserId1",
                table: "CV",
                column: "UserId1",
                unique: true,
                filter: "[UserId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CV_AspNetUsers_UserId1",
                table: "CV",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
