using asg1.DAL.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asg1.Models
{
    public class QuestionIndexViewModel
    {
        public string? Keyword { get; set; }
        public int? SubjectId { get; set; }
        public QuestionStatus? Status { get; set; }

        public IReadOnlyList<QuestionListItem> Items { get; set; } = Array.Empty<QuestionListItem>();
        public SelectList SubjectOptions { get; set; } = new SelectList(Enumerable.Empty<object>());
        public SelectList StatusOptions { get; set; } = new SelectList(Enumerable.Empty<object>());

        public class QuestionListItem
        {
            public int QuestionId { get; set; }
            public string Content { get; set; } = string.Empty;
            public string SubjectCode { get; set; } = string.Empty;
            public string SubjectName { get; set; } = string.Empty;
            public BloomLevel BloomLevel { get; set; }
            public QuestionStatus Status { get; set; }
            public DateTime CreatedAt { get; set; }
            public int RubricCount { get; set; }
        }
    }
}
