using asg1.BLL.Common;
using asg1.BLL.Dtos;
using asg1.BLL.Services;
using asg1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asg1.Controllers
{
    public class AiQuestionsController : Controller
    {
        private readonly IAiQuestionService _aiQuestionService;
        private readonly IQuestionService _questionService;

        public AiQuestionsController(IAiQuestionService aiQuestionService, IQuestionService questionService)
        {
            _aiQuestionService = aiQuestionService;
            _questionService = questionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = new AiQuestionGenerationViewModel();
            await PopulateSubjectsAsync(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AiQuestionGenerationViewModel vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSubjectsAsync(vm);
                return View(vm);
            }

            var document = vm.Document!;
            await using var stream = document.OpenReadStream();

            var result = await _aiQuestionService.GenerateAndSaveAsync(new AiQuestionGenerateDto
            {
                SubjectId = vm.SubjectId,
                Document = stream,
                FileName = document.FileName,
                ContentType = document.ContentType,
                FileSize = document.Length,
                QuestionCount = vm.QuestionCount
            }, cancellationToken);

            if (!result.IsSuccess)
            {
                ApplyErrors(result);
                await PopulateSubjectsAsync(vm);
                return View(vm);
            }

            vm.Questions = result.Value!;
            vm.SavedQuestionCount = vm.Questions.Count;
            vm.UploadedFileName = Path.GetFileName(document.FileName);
            await PopulateSubjectsAsync(vm);
            return View(vm);
        }

        private async Task PopulateSubjectsAsync(AiQuestionGenerationViewModel vm)
        {
            var subjects = await _questionService.GetSubjectOptionsAsync();
            vm.Subjects = subjects
                .Select(s => new SelectListItem(
                    $"{s.Code} - {s.Name}",
                    s.SubjectId.ToString(),
                    s.SubjectId == vm.SubjectId))
                .ToList();
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
