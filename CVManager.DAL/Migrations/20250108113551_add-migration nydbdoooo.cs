using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addmigrationnydbdoooo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Project",
                keyColumn: "ProjectId",
                keyValue: 1,
                column: "UploadDate",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Project",
                keyColumn: "ProjectId",
                keyValue: 1,
                column: "UploadDate",
                value: new DateTime(2025, 1, 8, 12, 34, 20, 4, DateTimeKind.Local).AddTicks(6048));
        }
    }
}
