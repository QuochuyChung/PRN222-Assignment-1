using asg1.Common.Enums;

namespace asg1.Common.Dtos
{
    public class QuestionSaveDto
    {
        public string Content { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public BloomLevel BloomLevel { get; set; }
    }
}
