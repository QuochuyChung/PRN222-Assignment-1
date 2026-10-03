using System.ComponentModel.DataAnnotations;
using asg1.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asg1.Models
{
    public class QuestionFormViewModel
    {
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Question content must not be empty.")]
        [StringLength(2000, ErrorMessage = "Question content must not exceed 2000 characters.")]
        [Display(Name = "Question content")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a subject.")]
        [Display(Name = "Subject")]
        public int? SubjectId { get; set; }

        [Display(Name = "Bloom level")]
        public BloomLevel BloomLevel { get; set; }
        [Display(Name = "Subject code")]
        public string SubjectCode { get; set; } = string.Empty;
        [Display(Name = "Subject name")]
        public string SubjectName { get; set; } = string.Empty;
        [Display(Name = "Status")]
        public QuestionStatus Status { get; set; }

        public SelectList SubjectOptions { get; set; } = new SelectList(Enumerable.Empty<object>());
    }
}
