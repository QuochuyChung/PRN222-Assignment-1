using asg1.Models.Entities;

namespace asg1.Models.Repositories
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<IEnumerable<Question>> GetQuestionsWithSubjectAsync();
        Task<(IEnumerable<Question> Items, int TotalCount)> GetQuestionsWithSubjectPaginatedAsync(int pageIndex, int pageSize, int? subjectId = null, string? searchString = null);
        Task<Question?> GetQuestionWithDetailsAsync(int id);
    }
}
