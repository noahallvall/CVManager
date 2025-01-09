using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class doooooot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CV_AspNetUsers_UserId",
                table: "CV");

            migrationBuilder.DropIndex(
                name: "IX_CV_UserId",
                table: "CV");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CV",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CV_UserId",
                table: "CV",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CV_AspNetUsers_UserId",
                table: "CV",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CV",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
    }
}
