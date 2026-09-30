using asg1.BLL.Common;
using asg1.BLL.Dtos;
using asg1.BLL.Services;
using asg1.Models;
using Microsoft.AspNetCore.Mvc;

namespace asg1.Controllers
{
    public class RubricsController : Controller
    {
        private readonly IRubricService _rubricService;
        private readonly IQuestionService _questionService;

        public RubricsController(IRubricService rubricService, IQuestionService questionService)
        {
            _rubricService = rubricService;
            _questionService = questionService;
        }

        public async Task<IActionResult> Create(int questionId)
        {
            var question = await _questionService.GetByIdAsync(questionId);
            if (question is null)
            {
                return NotFound();
            }

            var vm = new RubricFormViewModel
            {
                QuestionId = question.QuestionId,
                QuestionContent = question.Content,
                SubjectCode = question.SubjectCode,
                SubjectName = question.SubjectName,
                MaxScore = 1.0m
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RubricFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateQuestionContextAsync(vm);
                return View(vm);
            }

            var dto = new RubricSaveDto
            {
                QuestionId = vm.QuestionId,
                CriterionName = vm.CriterionName,
                Description = vm.Description,
                MaxScore = vm.MaxScore
            };

            var result = await _rubricService.CreateAsync(dto);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Đã thêm tiêu chí rubric thành công.";
                return RedirectToAction("Details", "Questions", new { id = vm.QuestionId });
            }

            ApplyErrors(result);
            await PopulateQuestionContextAsync(vm);
            return View(vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var rubric = await _rubricService.GetByIdAsync(id);
            if (rubric is null)
            {
                return NotFound();
            }

            var question = await _questionService.GetByIdAsync(rubric.QuestionId);

            var vm = new RubricFormViewModel
            {
                RubricCriterionId = rubric.RubricCriterionId,
                QuestionId = rubric.QuestionId,
                QuestionContent = question?.Content ?? string.Empty,
                SubjectCode = question?.SubjectCode ?? string.Empty,
                SubjectName = question?.SubjectName ?? string.Empty,
                CriterionName = rubric.CriterionName,
                Description = rubric.Description,
                MaxScore = rubric.MaxScore
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RubricFormViewModel vm)
        {
            if (id != vm.RubricCriterionId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await PopulateQuestionContextAsync(vm);
                return View(vm);
            }

            var dto = new RubricSaveDto
            {
                QuestionId = vm.QuestionId,
                CriterionName = vm.CriterionName,
                Description = vm.Description,
                MaxScore = vm.MaxScore
            };

            var result = await _rubricService.UpdateAsync(id, dto);
            if (result.Errors.ContainsKey(ErrorKeys.NotFound))
            {
                return NotFound();
            }

            if (result.IsSuccess)
            {
                TempData["Success"] = "Đã cập nhật tiêu chí rubric thành công.";
                return RedirectToAction("Details", "Questions", new { id = vm.QuestionId });
            }

            ApplyErrors(result);
            await PopulateQuestionContextAsync(vm);
            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var rubric = await _rubricService.GetByIdAsync(id);
            if (rubric is null)
            {
                return NotFound();
            }

            var question = await _questionService.GetByIdAsync(rubric.QuestionId);

            var vm = new RubricDeleteViewModel
            {
                RubricCriterionId = rubric.RubricCriterionId,
                QuestionId = rubric.QuestionId,
                QuestionContent = question?.Content ?? string.Empty,
                SubjectCode = question?.SubjectCode ?? string.Empty,
                SubjectName = question?.SubjectName ?? string.Empty,
                CriterionName = rubric.CriterionName,
                Description = rubric.Description,
                MaxScore = rubric.MaxScore
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rubric = await _rubricService.GetByIdAsync(id);
            if (rubric is null)
            {
                return NotFound();
            }

            var questionId = rubric.QuestionId;
            var result = await _rubricService.DeleteAsync(id);

            if (result.IsSuccess)
            {
                TempData["Success"] = "Đã xóa tiêu chí rubric thành công.";
                return RedirectToAction("Details", "Questions", new { id = questionId });
            }

            TempData["Error"] = "Không thể xóa tiêu chí rubric.";
            return RedirectToAction("Details", "Questions", new { id = questionId });
        }

        private async Task PopulateQuestionContextAsync(RubricFormViewModel vm)
        {
            var question = await _questionService.GetByIdAsync(vm.QuestionId);
            if (question is not null)
            {
                vm.QuestionContent = question.Content;
                vm.SubjectCode = question.SubjectCode;
                vm.SubjectName = question.SubjectName;
            }
        }

        private void ApplyErrors(ServiceResult result)
        {
            foreach (var (key, messages) in result.Errors)
            {
                foreach (var message in messages)
                {
                    ModelState.AddModelError(key, message);
                }
            }
        }
    }
}
