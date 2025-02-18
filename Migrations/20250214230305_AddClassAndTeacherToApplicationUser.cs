using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadingReviewSystem1207.Migrations
{
    /// <inheritdoc />
    public partial class AddClassAndTeacherToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Teacher",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Teacher",
                table: "AspNetUsers");
        }
    }
}
