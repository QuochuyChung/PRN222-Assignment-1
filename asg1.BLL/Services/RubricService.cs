using asg1.BLL.Common;
using asg1.BLL.Dtos;
using asg1.DAL.Entities;
using asg1.DAL.Repositories;

namespace asg1.BLL.Services
{
    public class RubricService : IRubricService
    {
        private const int MaxNameLength = 200;
        private const int MaxDescriptionLength = 500;
        private const decimal MaxScoreLimit = 100m;

        private readonly IRubricRepository _rubrics;
        private readonly IQuestionRepository _questions;

        public RubricService(IRubricRepository rubrics, IQuestionRepository questions)
        {
            _rubrics = rubrics;
            _questions = questions;
        }

        public async Task<IReadOnlyList<RubricDto>> GetByQuestionIdAsync(int questionId)
        {
            var entities = await _rubrics.GetByQuestionIdAsync(questionId);
            return entities.Select(Map).ToList();
        }

        public async Task<RubricDto?> GetByIdAsync(int id)
        {
            var entity = await _rubrics.GetByIdWithQuestionAsync(id);
            return entity is null ? null : Map(entity);
        }

        public async Task<ServiceResult<RubricDto>> CreateAsync(RubricSaveDto dto)
        {
            var errors = await ValidateAsync(dto, excludeId: null);
            if (errors.Count > 0)
            {
                return ServiceResult<RubricDto>.Failure(errors);
            }

            var entity = new RubricCriterion
            {
                QuestionId = dto.QuestionId,
                CriterionName = dto.CriterionName.Trim(),
                Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
                MaxScore = dto.MaxScore
            };

            await _rubrics.AddAsync(entity);
            await _rubrics.SaveChangesAsync();

            return ServiceResult<RubricDto>.Success(Map(entity));
        }

        public async Task<ServiceResult<RubricDto>> UpdateAsync(int id, RubricSaveDto dto)
        {
            var entity = await _rubrics.GetByIdWithQuestionAsync(id);
            if (entity is null)
            {
                return ServiceResult<RubricDto>.Failure(ErrorKeys.NotFound, "Không tìm thấy tiêu chí rubric.");
            }

            var errors = await ValidateAsync(dto, excludeId: id);
            if (errors.Count > 0)
            {
                return ServiceResult<RubricDto>.Failure(errors);
            }

            entity.CriterionName = dto.CriterionName.Trim();
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
            entity.MaxScore = dto.MaxScore;

            _rubrics.Update(entity);
            await _rubrics.SaveChangesAsync();

            return ServiceResult<RubricDto>.Success(Map(entity));
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var entity = await _rubrics.GetByIdWithQuestionAsync(id);
            if (entity is null)
            {
                return ServiceResult.Failure(ErrorKeys.NotFound, "Không tìm thấy tiêu chí rubric.");
            }

            _rubrics.Remove(entity);
            await _rubrics.SaveChangesAsync();

            return ServiceResult.Success();
        }

        private async Task<Dictionary<string, string[]>> ValidateAsync(RubricSaveDto dto, int? excludeId)
        {
            var errors = new Dictionary<string, string[]>();

            var question = await _questions.GetByIdWithDetailsAsync(dto.QuestionId);
            if (question is null)
            {
                errors["QuestionId"] = new[] { "Câu hỏi không tồn tại." };
            }

            var name = dto.CriterionName?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(name))
            {
                errors["CriterionName"] = new[] { "Tên tiêu chí không được để trống." };
            }
            else if (name.Length > MaxNameLength)
            {
                errors["CriterionName"] = new[] { $"Tên tiêu chí không được vượt quá {MaxNameLength} ký tự." };
            }

            if (!string.IsNullOrWhiteSpace(dto.Description) && dto.Description.Trim().Length > MaxDescriptionLength)
            {
                errors["Description"] = new[] { $"Mô tả tiêu chí không được vượt quá {MaxDescriptionLength} ký tự." };
            }

            if (dto.MaxScore <= 0)
            {
                errors["MaxScore"] = new[] { "Điểm tối đa phải lớn hơn 0." };
            }
            else if (dto.MaxScore > MaxScoreLimit)
            {
                errors["MaxScore"] = new[] { $"Điểm tối đa không được vượt quá {MaxScoreLimit}." };
            }

            if (errors.Count == 0 && await _rubrics.ExistsSameNameAsync(dto.QuestionId, name, excludeId))
            {
                errors["CriterionName"] = new[] { "Tiêu chí này đã tồn tại trong câu hỏi." };
            }

            return errors;
        }

        private static RubricDto Map(RubricCriterion entity) => new()
        {
            RubricCriterionId = entity.RubricCriterionId,
            QuestionId = entity.QuestionId,
            CriterionName = entity.CriterionName,
            Description = entity.Description,
            MaxScore = entity.MaxScore
        };
    }
}
