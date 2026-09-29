namespace asg1.BLL.Dtos
{
    public class RubricDto
    {
        public int RubricCriterionId { get; set; }
        public string CriterionName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal MaxScore { get; set; }
    }
}
