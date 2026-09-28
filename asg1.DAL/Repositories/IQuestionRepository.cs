using asg1.DAL.Entities;
using asg1.DAL.Enums;

namespace asg1.DAL.Repositories
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<IReadOnlyList<Question>> SearchAsync(int? subjectId, QuestionStatus? status, string? keyword);
        Task<Question?> GetByIdWithDetailsAsync(int id);
        Task<bool> ExistsSameContentAsync(int subjectId, string content, int? excludeQuestionId);
    }
}
