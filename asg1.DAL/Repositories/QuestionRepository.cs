using asg1.DAL.Data;
using asg1.DAL.Entities;
using asg1.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace asg1.DAL.Repositories
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Question>> SearchAsync(int? subjectId, QuestionStatus? status, string? keyword)
        {
            IQueryable<Question> query = _dbSet
                .Include(q => q.Subject)
                .Include(q => q.RubricCriteria);

            if (subjectId.HasValue)
            {
                query = query.Where(q => q.SubjectId == subjectId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(q => q.Status == status.Value);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var term = keyword.Trim();
                query = query.Where(q => q.Content.Contains(term));
            }

            return await query
                .OrderByDescending(q => q.CreatedAt)
                .ThenByDescending(q => q.QuestionId)
                .ToListAsync();
        }

        public async Task<Question?> GetByIdWithDetailsAsync(int id) =>
            await _dbSet
                .Include(q => q.Subject)
                .Include(q => q.RubricCriteria)
                .FirstOrDefaultAsync(q => q.QuestionId == id);

        public async Task<bool> ExistsSameContentAsync(int subjectId, string content, int? excludeQuestionId)
        {
            var normalized = content.Trim();
            return await _dbSet.AnyAsync(q =>
                q.SubjectId == subjectId &&
                q.Content == normalized &&
                (!excludeQuestionId.HasValue || q.QuestionId != excludeQuestionId.Value));
        }
    }
}
