using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ReadingReviewSystem1207.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;  // 新增姓名欄位

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "密碼與確認密碼不符")]
        public string ConfirmPassword { get; set; } = string.Empty;

        // 學號（選填）
        public string? StudentId { get; set; }

        // 申請教師身份（勾選）
        public bool IsTeacher { get; set; }

        // 教師證上傳（非必填，僅在 IsTeacher 為 true 時需要）
        public IFormFile? TeacherCertificate { get; set; } = null;

        // 管理員代碼（僅管理員使用，註冊頁面可以選擇不填）
        public string? AdminCode { get; set; }
    }
}
