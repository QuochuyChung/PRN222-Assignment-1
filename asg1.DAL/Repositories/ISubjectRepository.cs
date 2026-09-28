using asg1.DAL.Entities;

namespace asg1.DAL.Repositories
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        Task<Subject?> GetByCodeAsync(string code);
    }
}
