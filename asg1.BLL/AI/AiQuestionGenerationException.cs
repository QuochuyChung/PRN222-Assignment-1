namespace asg1.BLL.AI
{
    public class AiQuestionGenerationException : Exception
    {
        public AiQuestionGenerationException(string message) : base(message)
        {
        }

        public AiQuestionGenerationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
