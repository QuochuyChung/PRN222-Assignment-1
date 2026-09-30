using asg1.DAL.Data;
using asg1.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace asg1.DAL.Repositories
{
    public class RubricRepository : GenericRepository<RubricCriterion>, IRubricRepository
    {
        public RubricRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<RubricCriterion>> GetByQuestionIdAsync(int questionId)
        {
            return await _dbSet
                .Where(r => r.QuestionId == questionId)
                .OrderBy(r => r.RubricCriterionId)
                .ToListAsync();
        }

        public async Task<RubricCriterion?> GetByIdWithQuestionAsync(int id)
        {
            return await _dbSet
                .Include(r => r.Question)
                    .ThenInclude(q => q!.Subject)
                .FirstOrDefaultAsync(r => r.RubricCriterionId == id);
        }

        public async Task<bool> ExistsSameNameAsync(int questionId, string criterionName, int? excludeId)
        {
            var normalized = criterionName.Trim();
            return await _dbSet.AnyAsync(r =>
                r.QuestionId == questionId &&
                r.CriterionName == normalized &&
                (!excludeId.HasValue || r.RubricCriterionId != excludeId.Value));
        }
    }
}
