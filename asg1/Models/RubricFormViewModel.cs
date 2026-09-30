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

        [Required(ErrorMessage = "Tên tiêu chí không được để trống.")]
        [StringLength(200, ErrorMessage = "Tên tiêu chí không được vượt quá 200 ký tự.")]
        [Display(Name = "Tên tiêu chí")]
        public string CriterionName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Mô tả tiêu chí không được vượt quá 500 ký tự.")]
        [Display(Name = "Mô tả / Hướng dẫn chấm")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập điểm tối đa.")]
        [Range(0.01, 100.00, ErrorMessage = "Điểm tối đa phải từ 0.01 đến 100.00.")]
        [Display(Name = "Điểm tối đa")]
        public decimal MaxScore { get; set; } = 1.0m;
    }
}
