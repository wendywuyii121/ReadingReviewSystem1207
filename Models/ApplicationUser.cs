using Microsoft.AspNetCore.Identity;

namespace ReadingReviewSystem1207.Models
{
    public class ApplicationUser : IdentityUser
    {
        // 新增：確保 Name 屬性不為 null
        public string Name { get; set; } = string.Empty;

        // 新增：表示使用者身份（例如 "Student" 或 "Teacher"），預設不為 null
        public string Role { get; set; } = string.Empty;

        // 新增：選填的學號
        public string? StudentId { get; set; }

        // 新增：管理教師審核狀態，預設為 "Pending"
        public string Status { get; set; } = "Pending";

        // 新增：選填，教師證上傳檔案路徑
        public string? TeacherCertificateUrl { get; set; }

        //✅ **新增管理員驗證狀態**
        public bool IsAdminVerified { get; set; } = false;

       }
}
