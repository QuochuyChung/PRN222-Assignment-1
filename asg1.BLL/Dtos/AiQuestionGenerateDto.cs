namespace asg1.BLL.Dtos
{
    public class AiQuestionGenerateDto
    {
        public int SubjectId { get; set; }
        public Stream Document { get; set; } = Stream.Null;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public int QuestionCount { get; set; }
    }
}
