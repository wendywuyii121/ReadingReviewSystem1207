using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadingReviewSystem1207.Migrations
{
    public partial class TransferFullNameToName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 新增 Name 欄位（如果尚未存在）
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // 將 FullName 欄位資料複製到 Name 欄位
            migrationBuilder.Sql("UPDATE AspNetUsers SET Name = FullName");

            // 刪除 FullName 欄位
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 在回滾時，重新新增 FullName 欄位
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // 將 Name 欄位的資料複製回 FullName
            migrationBuilder.Sql("UPDATE AspNetUsers SET FullName = Name");

            // (視需求而定，你也可以選擇回滾時保留 Name 欄位)
        }
    }
}
