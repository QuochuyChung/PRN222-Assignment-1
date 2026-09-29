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

        public async Task<Question?> GetQuestionWithDetailsAsync(int id)
        {
            return await _dbSet.Include(q => q.Subject)
                               .FirstOrDefaultAsync(q => q.QuestionId == id);
        }
    }
}
