using asg1.BLL.Common;
using asg1.Common.Dtos;
using asg1.DAL.Entities;
using asg1.Common.Enums;
using asg1.DAL.Repositories;

namespace asg1.BLL.Services
{
    public class QuestionService : IQuestionService
    {
        private const int MaxContentLength = 2000;

        private readonly IQuestionRepository _questions;
        private readonly ISubjectRepository _subjects;

        public QuestionService(IQuestionRepository questions, ISubjectRepository subjects)
        {
            _questions = questions;
            _subjects = subjects;
        }

        public async Task<IReadOnlyList<QuestionDto>> SearchAsync(QuestionQuery query)
        {
            var entities = await _questions.SearchAsync(query.SubjectId, query.Status, query.Keyword);
            return entities.Select(Map).ToList();
        }

        public async Task<QuestionDto?> GetByIdAsync(int id)
        {
            var entity = await _questions.GetByIdWithDetailsAsync(id);
            return entity is null ? null : Map(entity);
        }

        public async Task<IReadOnlyList<SubjectLookupDto>> GetSubjectOptionsAsync()
        {
            var subjects = await _subjects.GetAllAsync();
            return subjects
                .OrderBy(s => s.Code)
                .Select(s => new SubjectLookupDto
                {
                    SubjectId = s.SubjectId,
                    Code = s.Code,
                    Name = s.Name
                })
                .ToList();
        }

        public async Task<ServiceResult<QuestionDto>> CreateAsync(QuestionSaveDto dto)
        {
            var errors = await ValidateAsync(dto, excludeQuestionId: null);
            if (errors.Count > 0)
            {
                return ServiceResult<QuestionDto>.Failure(errors);
            }

            var entity = new Question
            {
                SubjectId = dto.SubjectId,
                Content = dto.Content.Trim(),
                BloomLevel = dto.BloomLevel,
                Status = QuestionStatus.Draft,
                Source = QuestionSource.Manual,
                CreatedAt = DateTime.UtcNow
            };

            await _questions.AddAsync(entity);
            await _questions.SaveChangesAsync();

            return ServiceResult<QuestionDto>.Success(Map(entity));
        }

        public async Task<ServiceResult<QuestionDto>> UpdateAsync(int id, QuestionSaveDto dto)
        {
            var entity = await _questions.GetByIdWithDetailsAsync(id);
            if (entity is null)
            {
                return ServiceResult<QuestionDto>.Failure(ErrorKeys.NotFound, "Question not found.");
            }

            var errors = await ValidateAsync(dto, excludeQuestionId: id);
            if (errors.Count > 0)
            {
                return ServiceResult<QuestionDto>.Failure(errors);
            }

            entity.SubjectId = dto.SubjectId;
            entity.Content = dto.Content.Trim();
            entity.BloomLevel = dto.BloomLevel;

            _questions.Update(entity);
            await _questions.SaveChangesAsync();

            return ServiceResult<QuestionDto>.Success(Map(entity));
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var entity = await _questions.GetByIdWithDetailsAsync(id);
            if (entity is null)
            {
                return ServiceResult.Failure(ErrorKeys.NotFound, "Question not found.");
            }

            _questions.Remove(entity);
            await _questions.SaveChangesAsync();

            return ServiceResult.Success();
        }

        private async Task<Dictionary<string, string[]>> ValidateAsync(QuestionSaveDto dto, int? excludeQuestionId)
        {
            var errors = new Dictionary<string, string[]>();

            var content = dto.Content?.Trim() ?? string.Empty;
            if (content.Length == 0)
            {
                errors["Content"] = new[] { "Question content must not be empty." };
            }
            else if (content.Length > MaxContentLength)
            {
                errors["Content"] = new[] { $"Question content must not exceed {MaxContentLength} characters." };
            }

            if (!Enum.IsDefined(dto.BloomLevel))
            {
                errors["BloomLevel"] = new[] { "Invalid Bloom level." };
            }

            var subjectExists = dto.SubjectId > 0 &&
                (await _subjects.GetByIdAsync(dto.SubjectId)) is not null;
            if (!subjectExists)
            {
                errors["SubjectId"] = new[] { "Invalid subject." };
            }

            if (errors.Count == 0 &&
                await _questions.ExistsSameContentAsync(dto.SubjectId, content, excludeQuestionId))
            {
                errors["Content"] = new[] { "This question content already exists for this subject." };
            }

            return errors;
        }

        private static QuestionDto Map(Question entity) => new()
        {
            QuestionId = entity.QuestionId,
            SubjectId = entity.SubjectId,
            SubjectCode = entity.Subject?.Code ?? string.Empty,
            SubjectName = entity.Subject?.Name ?? string.Empty,
            Content = entity.Content,
            BloomLevel = entity.BloomLevel,
            Status = entity.Status,
            Source = entity.Source,
            CreatedAt = entity.CreatedAt,
            RubricCount = entity.RubricCriteria?.Count ?? 0,
            RubricCriteria = (entity.RubricCriteria ?? new List<RubricCriterion>())
                .Select(r => new RubricDto
                {
                    RubricCriterionId = r.RubricCriterionId,
                    QuestionId = r.QuestionId,
                    CriterionName = r.CriterionName,
                    Description = r.Description,
                    MaxScore = r.MaxScore
                })
                .ToList()
        };
    }
}
