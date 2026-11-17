using asg1.Common.Results;
using asg1.Common.Dtos;

namespace asg1.BLL.Services
{
    public interface IRubricService
    {
        Task<IReadOnlyList<RubricDto>> GetByQuestionIdAsync(int questionId);
        Task<RubricDto?> GetByIdAsync(int id);
        Task<ServiceResult<RubricDto>> CreateAsync(RubricSaveDto dto);
        Task<ServiceResult<RubricDto>> UpdateAsync(int id, RubricSaveDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
