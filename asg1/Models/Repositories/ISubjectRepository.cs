using asg1.Models.Entities;

namespace asg1.Models.Repositories
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        Task<Subject?> GetByCodeAsync(string code);
    }
}
