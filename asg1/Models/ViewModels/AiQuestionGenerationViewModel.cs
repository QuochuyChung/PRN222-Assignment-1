using System.ComponentModel.DataAnnotations;
using asg1.Models.AI;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asg1.Models.ViewModels;

public sealed class AiQuestionGenerationViewModel
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
    public IReadOnlyList<SelectListItem> Subjects { get; set; } = [];
    public IReadOnlyList<GeneratedQuestionDraft> Questions { get; set; } = [];
}
