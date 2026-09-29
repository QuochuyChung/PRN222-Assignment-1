using asg1.Models.Data;
using asg1.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace asg1.Models.Repositories
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Question>> GetQuestionsWithSubjectAsync()
        {
            return await _dbSet.Include(q => q.Subject).ToListAsync();
        }

        public async Task<(IEnumerable<Question> Items, int TotalCount)> GetQuestionsWithSubjectPaginatedAsync(int pageIndex, int pageSize, int? subjectId = null, string? searchString = null)
        {
            var query = _dbSet.AsQueryable();
            if (subjectId.HasValue)
            {
                query = query.Where(q => q.SubjectId == subjectId.Value);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(q => q.Content.Contains(searchString));
            }

            var totalCount = await query.CountAsync();
            var items = await query.Include(q => q.Subject)
                                    .OrderByDescending(q => q.CreatedAt)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
            return (items, totalCount);
        }

        public async Task<Question?> GetQuestionWithDetailsAsync(int id)
        {
            return await _dbSet.Include(q => q.Subject)
                               .FirstOrDefaultAsync(q => q.QuestionId == id);
        }
    }
}
