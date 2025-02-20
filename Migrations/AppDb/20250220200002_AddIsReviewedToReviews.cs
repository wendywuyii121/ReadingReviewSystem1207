using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadingReviewSystem1207.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddIsReviewedToReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReviewed",
                table: "Reviews",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReviewed",
                table: "Reviews");
        }
    }
}
