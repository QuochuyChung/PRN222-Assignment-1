using asg1.DAL.Enums;

namespace asg1.BLL.Dtos
{
    public class GeneratedQuestionDto
    {
        public string Content { get; set; } = string.Empty;
        public string SuggestedAnswer { get; set; } = string.Empty;
        public BloomLevel BloomLevel { get; set; }
    }
}
