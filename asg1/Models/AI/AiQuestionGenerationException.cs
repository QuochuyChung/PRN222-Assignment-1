namespace asg1.Models.AI;

public sealed class AiQuestionGenerationException : Exception
{
    public AiQuestionGenerationException(string message) : base(message)
    {
    }

    public AiQuestionGenerationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
