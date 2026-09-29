using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asg1.DAL.Entities
{
    public class RubricCriterion
    {
        public int RubricCriterionId { get; set; }

        public int QuestionId { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public Question? Question { get; set; }

        [Required, MaxLength(200)]
        public string CriterionName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public decimal MaxScore { get; set; }
    }
}
