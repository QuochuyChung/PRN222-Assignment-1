using asg1.Common.Results;
using asg1.Common.Dtos;

namespace asg1.BLL.Services
{
    public interface IAiQuestionService
    {
        Task<ServiceResult<IReadOnlyList<GeneratedQuestionDto>>> GenerateAndSaveAsync(
            AiQuestionGenerateDto dto,
            CancellationToken cancellationToken = default);
    }
}
