using asg1.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace asg1.DAL.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider sp)
        {
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (await db.Subjects.AnyAsync())
            {
                return;
            }

            db.Subjects.AddRange(
                new Subject { Code = "PRN222", Name = "Lập trình .NET Core" },
                new Subject { Code = "PRF192", Name = "Lập trình C++" },
                new Subject { Code = "ITE103", Name = "Cơ sở dữ liệu" });

            await db.SaveChangesAsync();
        }
    }
}
