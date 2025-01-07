using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CVManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class komIgen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CV",
                columns: new[] { "CVId", "Address", "Email", "FirstName", "LastName", "Phone", "ProfilePicturePath", "Summary", "UserId" },
                values: new object[] { 1, "Storgatan 3", "ClarkS@gmail.com", "Clark", "Smith", "7329329932", null, "Clark är en rolig grabb", null });

            migrationBuilder.InsertData(
                table: "Project",
                columns: new[] { "ProjectId", "ProjectDescription", "ProjectName" },
                values: new object[] { 1, "En databas för Aliens", "MIB" });

            migrationBuilder.InsertData(
                table: "Education",
                columns: new[] { "EducationId", "CVId", "EducationName", "Institution" },
                values: new object[,]
                {
                    { 1, 1, "Systemvetenskap", "Orebro" },
                    { 2, 1, "Webbutvecklare", "KTH" }
                });

            migrationBuilder.InsertData(
                table: "Experience",
                columns: new[] { "ExperienceId", "CVId", "CompanyName", "Role" },
                values: new object[,]
                {
                    { 1, 1, "Walmart", "Casher" },
                    { 2, 1, "Google", "Programmer" }
                });

            migrationBuilder.InsertData(
                table: "Skill",
                columns: new[] { "SkillId", "CVId", "SkillName" },
                values: new object[,]
                {
                    { 1, 1, "C#" },
                    { 2, 1, "JavaSript" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Education",
                keyColumn: "EducationId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Education",
                keyColumn: "EducationId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Experience",
                keyColumn: "ExperienceId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Experience",
                keyColumn: "ExperienceId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Project",
                keyColumn: "ProjectId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Skill",
                keyColumn: "SkillId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Skill",
                keyColumn: "SkillId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CV",
                keyColumn: "CVId",
                keyValue: 1);
        }
    }
}
