using System.ComponentModel.DataAnnotations;
using asg1.BLL.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asg1.Models
{
    public class AiQuestionGenerationViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn môn học.")]
        [Display(Name = "Môn học")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn tài liệu.")]
        [Display(Name = "Tài liệu")]
        public IFormFile? Document { get; set; }

        [Range(1, 20, ErrorMessage = "Số câu hỏi phải từ 1 đến 20.")]
        [Display(Name = "Số câu hỏi")]
        public int QuestionCount { get; set; } = 5;

        public string? UploadedFileName { get; set; }
        public int SavedQuestionCount { get; set; }
        public IReadOnlyList<SelectListItem> Subjects { get; set; } = new List<SelectListItem>();
        public IReadOnlyList<GeneratedQuestionDto> Questions { get; set; } = new List<GeneratedQuestionDto>();
    }
}
