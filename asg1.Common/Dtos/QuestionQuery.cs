using asg1.Common.Enums;

namespace asg1.Common.Dtos
{
    public class QuestionQuery
    {
        public string? Keyword { get; set; }
        public int? SubjectId { get; set; }
        public QuestionStatus? Status { get; set; }
    }
}
