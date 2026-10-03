using asg1.BLL.AI;
using asg1.BLL.Common;
using asg1.Common.Dtos;
using asg1.DAL.Entities;
using asg1.Common.Enums;
using asg1.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace asg1.BLL.Services
{
    public class AiQuestionService : IAiQuestionService
    {
        private const long MaxFileSize = 20 * 1024 * 1024;
        private const int MinQuestionCount = 1;
        private const int MaxQuestionCount = 20;

        private static readonly HashSet<string> SupportedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf", ".doc", ".docx", ".ppt", ".pptx", ".txt", ".md"
        };

        private readonly IAiQuestionGenerator _generator;
        private readonly IQuestionRepository _questions;
        private readonly ISubjectRepository _subjects;

        public AiQuestionService(
            IAiQuestionGenerator generator,
            IQuestionRepository questions,
            ISubjectRepository subjects)
        {
            _generator = generator;
            _questions = questions;
            _subjects = subjects;
        }

        public async Task<ServiceResult<IReadOnlyList<GeneratedQuestionDto>>> GenerateAndSaveAsync(
            AiQuestionGenerateDto dto,
            CancellationToken cancellationToken = default)
        {
            var errors = await ValidateAsync(dto);
            if (errors.Count > 0)
            {
                return ServiceResult<IReadOnlyList<GeneratedQuestionDto>>.Failure(errors);
            }

            IReadOnlyList<GeneratedQuestionDto> generated;
            try
            {
                generated = await _generator.GenerateAsync(
                    dto.Document,
                    dto.FileName,
                    dto.ContentType,
                    dto.QuestionCount,
                    cancellationToken);
            }
            catch (AiQuestionGenerationException exception)
            {
                return ServiceResult<IReadOnlyList<GeneratedQuestionDto>>.Failure(string.Empty, exception.Message);
            }

            foreach (var question in generated)
            {
                await _questions.AddAsync(new Question
                {
                    SubjectId = dto.SubjectId,
                    Content = question.Content.Trim(),
                    BloomLevel = question.BloomLevel,
                    Source = QuestionSource.AIGenerated,
                    Status = QuestionStatus.Draft,
                    CreatedAt = DateTime.UtcNow
                });
            }

            try
            {
                await _questions.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return ServiceResult<IReadOnlyList<GeneratedQuestionDto>>.Failure(
                    string.Empty,
                    "Questions were generated but could not be saved to the database. Please check the migrations and the DB connection.");
            }

            return ServiceResult<IReadOnlyList<GeneratedQuestionDto>>.Success(generated);
        }

        private async Task<Dictionary<string, string[]>> ValidateAsync(AiQuestionGenerateDto dto)
        {
            var errors = new Dictionary<string, string[]>();

            if (dto.FileSize <= 0)
            {
                errors["Document"] = new[] { "Please select a document with content." };
            }
            else if (dto.FileSize > MaxFileSize)
            {
                errors["Document"] = new[] { "The document must not exceed 20 MB." };
            }
            else if (!SupportedExtensions.Contains(Path.GetExtension(dto.FileName)))
            {
                errors["Document"] = new[] { "Only PDF, Word, PowerPoint, TXT and Markdown are supported." };
            }

            if (dto.QuestionCount < MinQuestionCount || dto.QuestionCount > MaxQuestionCount)
            {
                errors["QuestionCount"] = new[] { $"Question count must be between {MinQuestionCount} and {MaxQuestionCount}." };
            }

            var subjectExists = dto.SubjectId > 0 &&
                (await _subjects.GetByIdAsync(dto.SubjectId)) is not null;
            if (!subjectExists)
            {
                errors["SubjectId"] = new[] { "The selected subject does not exist." };
            }

            return errors;
        }
    }
}
