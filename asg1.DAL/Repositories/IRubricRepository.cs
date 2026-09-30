using asg1.DAL.Entities;

namespace asg1.DAL.Repositories
{
    public interface IRubricRepository : IGenericRepository<RubricCriterion>
    {
        Task<IReadOnlyList<RubricCriterion>> GetByQuestionIdAsync(int questionId);
        Task<RubricCriterion?> GetByIdWithQuestionAsync(int id);
        Task<bool> ExistsSameNameAsync(int questionId, string criterionName, int? excludeId);
    }
}
