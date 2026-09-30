using asg1.BLL.Common;
using asg1.BLL.Dtos;

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
