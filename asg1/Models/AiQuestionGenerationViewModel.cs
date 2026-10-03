using System.ComponentModel.DataAnnotations;
using asg1.Common.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asg1.Models
{
    public class AiQuestionGenerationViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please select a subject.")]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Please select a document.")]
        [Display(Name = "Document")]
        public IFormFile? Document { get; set; }

        [Range(1, 20, ErrorMessage = "Question count must be between 1 and 20.")]
        [Display(Name = "Question count")]
        public int QuestionCount { get; set; } = 5;

        public string? UploadedFileName { get; set; }
        public int SavedQuestionCount { get; set; }
        public IReadOnlyList<SelectListItem> Subjects { get; set; } = new List<SelectListItem>();
        public IReadOnlyList<GeneratedQuestionDto> Questions { get; set; } = new List<GeneratedQuestionDto>();
    }
}
