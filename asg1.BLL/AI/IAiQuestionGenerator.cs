using asg1.BLL.Dtos;

namespace asg1.BLL.AI
{
    public interface IAiQuestionGenerator
    {
        Task<IReadOnlyList<GeneratedQuestionDto>> GenerateAsync(
            Stream document,
            string fileName,
            string contentType,
            int questionCount,
            CancellationToken cancellationToken = default);
    }
}
