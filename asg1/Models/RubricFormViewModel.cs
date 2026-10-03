using System.ComponentModel.DataAnnotations;

namespace asg1.Models
{
    public class RubricFormViewModel
    {
        public int RubricCriterionId { get; set; }

        public int QuestionId { get; set; }

        public string QuestionContent { get; set; } = string.Empty;

        public string SubjectCode { get; set; } = string.Empty;

        public string SubjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Criterion name must not be empty.")]
        [StringLength(200, ErrorMessage = "Criterion name must not exceed 200 characters.")]
        [Display(Name = "Criterion name")]
        public string CriterionName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Criterion description must not exceed 500 characters.")]
        [Display(Name = "Description / Grading guide")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please enter the maximum score.")]
        [Range(0.01, 100.00, ErrorMessage = "Maximum score must be between 0.01 and 100.00.")]
        [Display(Name = "Max score")]
        public decimal MaxScore { get; set; } = 1.0m;
    }
}
