namespace asg1.Models
{
    public class RubricDeleteViewModel
    {
        public int RubricCriterionId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string CriterionName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal MaxScore { get; set; }
    }
}
