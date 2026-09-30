using asg1.BLL.Common;
using asg1.BLL.Dtos;

namespace asg1.BLL.Services
{
    public interface IQuestionService
    {
        Task<(IReadOnlyList<QuestionDto> Items, int TotalCount)> SearchAsync(QuestionQuery query);
        Task<QuestionDto?> GetByIdAsync(int id);
        Task<IReadOnlyList<SubjectLookupDto>> GetSubjectOptionsAsync();
        Task<ServiceResult<QuestionDto>> CreateAsync(QuestionSaveDto dto);
        Task<ServiceResult<QuestionDto>> UpdateAsync(int id, QuestionSaveDto dto);
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult> ChangeStatusAsync(int id, asg1.DAL.Enums.QuestionStatus status);
    }
}
