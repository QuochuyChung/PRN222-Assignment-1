using asg1.Models.Entities;

namespace asg1.Models.Repositories
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<IEnumerable<Question>> GetQuestionsWithSubjectAsync();
        Task<Question?> GetQuestionWithDetailsAsync(int id);
    }
}
