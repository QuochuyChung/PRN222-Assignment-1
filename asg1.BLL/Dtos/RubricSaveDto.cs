namespace asg1.BLL.Dtos
{
    public class RubricSaveDto
    {
        public int QuestionId { get; set; }
        public string CriterionName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal MaxScore { get; set; }
    }
}
