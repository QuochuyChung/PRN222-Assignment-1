using System.ComponentModel.DataAnnotations;
using asg1.DAL.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asg1.Models
{
    public class QuestionFormViewModel
    {
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Nội dung câu hỏi không được để trống.")]
        [StringLength(2000, ErrorMessage = "Nội dung câu hỏi không được vượt quá 2000 ký tự.")]
        [Display(Name = "Nội dung câu hỏi")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn môn học.")]
        [Display(Name = "Môn học")]
        public int? SubjectId { get; set; }

        [Display(Name = "Mức độ Bloom")]
        public BloomLevel BloomLevel { get; set; }
        [Display(Name = "Mã môn")]
        public string SubjectCode { get; set; } = string.Empty;
        [Display(Name = "Tên môn")]
        public string SubjectName { get; set; } = string.Empty;
        [Display(Name = "Trạng thái")]
        public QuestionStatus Status { get; set; }

        public SelectList SubjectOptions { get; set; } = new SelectList(Enumerable.Empty<object>());
    }
}
