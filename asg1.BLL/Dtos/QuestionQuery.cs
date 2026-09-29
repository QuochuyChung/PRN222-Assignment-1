using asg1.DAL.Enums;

namespace asg1.BLL.Dtos
{
    public class QuestionQuery
    {
        public string? Keyword { get; set; }
        public int? SubjectId { get; set; }
        public QuestionStatus? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
