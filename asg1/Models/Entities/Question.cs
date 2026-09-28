using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using asg1.Models.Enums;

namespace asg1.Models.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }

        public int SubjectId { get; set; }

        [ForeignKey(nameof(SubjectId))]
        public Subject? Subject { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public BloomLevel BloomLevel { get; set; }

        public QuestionStatus Status { get; set; } = QuestionStatus.Draft;

        public QuestionSource Source { get; set; } = QuestionSource.Manual;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<RubricCriterion> RubricCriteria { get; set; } = new List<RubricCriterion>();
    }
}
