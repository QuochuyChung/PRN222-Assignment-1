namespace asg1.Models.AI;

public interface IAiQuestionGenerator
{
    Task<IReadOnlyList<GeneratedQuestionDraft>> GenerateAsync(
        Stream document,
        string fileName,
        string contentType,
        int questionCount,
        CancellationToken cancellationToken = default);
}
