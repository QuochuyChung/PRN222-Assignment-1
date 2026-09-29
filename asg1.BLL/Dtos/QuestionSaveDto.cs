using asg1.DAL.Enums;

namespace asg1.BLL.Dtos
{
    public class QuestionSaveDto
    {
        public string Content { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public BloomLevel BloomLevel { get; set; }
    }
}
