using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class uppdatrademessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_CV_CVId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_CVId",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "CVId",
                table: "Message",
                newName: "CVSentId");

            migrationBuilder.AddColumn<int>(
                name: "CVRecievedId",
                table: "Message",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Message_CVRecievedId",
                table: "Message",
                column: "CVRecievedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_CV_CVRecievedId",
                table: "Message",
                column: "CVRecievedId",
                principalTable: "CV",
                principalColumn: "CVId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_CV_CVRecievedId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_CVRecievedId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "CVRecievedId",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "CVSentId",
                table: "Message",
                newName: "CVId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_CVId",
                table: "Message",
                column: "CVId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_CV_CVId",
                table: "Message",
                column: "CVId",
                principalTable: "CV",
                principalColumn: "CVId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
