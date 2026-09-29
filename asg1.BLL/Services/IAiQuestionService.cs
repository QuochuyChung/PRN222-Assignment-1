using asg1.BLL.Common;
using asg1.BLL.Dtos;

namespace asg1.BLL.Services
{
    public interface IAiQuestionService
    {
        Task<ServiceResult<IReadOnlyList<GeneratedQuestionDto>>> GenerateAndSaveAsync(
            AiQuestionGenerateDto dto,
            CancellationToken cancellationToken = default);
    }
}
