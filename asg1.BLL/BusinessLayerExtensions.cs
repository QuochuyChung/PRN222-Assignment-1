using asg1.BLL.Services;
using asg1.DAL.Data;
using asg1.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace asg1.BLL
{
    /// <summary>
    /// Điểm vào duy nhất để Presentation lắp BLL (và DAL phía dưới) vào DI,
    /// nhờ vậy project Presentation không cần biết gì về DAL.
    /// </summary>
    public static class BusinessLayerExtensions
    {
        public static IServiceCollection AddBusinessLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DAL: DbContext + Repository
            services.AddAppDbContext(configuration);
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IRubricRepository, RubricRepository>();

            // BLL: Service
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAiQuestionService, AiQuestionService>();
            services.AddScoped<IRubricService, RubricService>();

            return services;
        }

        public static Task SeedDataAsync(this IServiceProvider services) =>
            DbSeeder.SeedAsync(services);
    }
}
