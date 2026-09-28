using asg1.DAL.Enums;

namespace asg1.BLL.Dtos
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public BloomLevel BloomLevel { get; set; }
        public QuestionStatus Status { get; set; }
        public QuestionSource Source { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RubricCount { get; set; }
        public IReadOnlyList<RubricDto> RubricCriteria { get; set; } = Array.Empty<RubricDto>();
    }
}
