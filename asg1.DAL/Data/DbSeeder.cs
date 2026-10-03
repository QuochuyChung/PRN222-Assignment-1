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
                new Subject { Code = "PRN222", Name = ".NET Core Programming" },
                new Subject { Code = "PRF192", Name = "C++ Programming" },
                new Subject { Code = "ITE103", Name = "Database Systems" });

            await db.SaveChangesAsync();
        }
    }
}
