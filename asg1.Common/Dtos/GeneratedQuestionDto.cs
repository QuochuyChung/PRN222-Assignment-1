using asg1.Common.Enums;

namespace asg1.Common.Dtos
{
    public class GeneratedQuestionDto
    {
        public string Content { get; set; } = string.Empty;
        public string SuggestedAnswer { get; set; } = string.Empty;
        public BloomLevel BloomLevel { get; set; }
    }
}
