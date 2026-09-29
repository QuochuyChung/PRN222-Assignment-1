using asg1.DAL.Data;
using asg1.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace asg1.DAL.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Subject?> GetByCodeAsync(string code) =>
            await _dbSet.FirstOrDefaultAsync(s => s.Code == code);
    }
}
