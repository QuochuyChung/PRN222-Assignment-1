using System.ComponentModel.DataAnnotations;
using asg1.BLL.Common;
using asg1.BLL.Dtos;
using asg1.BLL.Services;
using asg1.DAL.Enums;
using asg1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asg1.Controllers
{
    public class QuestionsController : Controller
    {
        private const int PreviewLength = 120;

        private readonly IQuestionService _service;

        public QuestionsController(IQuestionService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string? keyword, int? subjectId, QuestionStatus? status)
        {
            var questions = await _service.SearchAsync(new QuestionQuery
            {
                Keyword = keyword,
                SubjectId = subjectId,
                Status = status
            });

            var vm = new QuestionIndexViewModel
            {
                Keyword = keyword,
                SubjectId = subjectId,
                Status = status,
                SubjectOptions = await BuildSubjectOptionsAsync("Tất cả môn học", subjectId),
                StatusOptions = BuildStatusOptions(status),
                Items = questions.Select(q => new QuestionIndexViewModel.QuestionListItem
                {
                    QuestionId = q.QuestionId,
                    Content = Truncate(q.Content),
                    SubjectCode = q.SubjectCode,
                    SubjectName = q.SubjectName,
                    BloomLevel = q.BloomLevel,
                    Status = q.Status,
                    CreatedAt = q.CreatedAt,
                    RubricCount = q.RubricCount
                }).ToList()
            };

            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var question = await _service.GetByIdAsync(id);
            if (question is null)
            {
                return NotFound();
            }

            return View(ToDetailsVm(question));
        }

        public async Task<IActionResult> Create()
        {
            var vm = new QuestionFormViewModel
            {
                SubjectOptions = await BuildSubjectOptionsAsync("Chọn môn học")
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.SubjectOptions = await BuildSubjectOptionsAsync("Chọn môn học", vm.SubjectId);
                return View(vm);
            }

            var result = await _service.CreateAsync(ToSaveDto(vm));
            if (result.IsSuccess)
            {
                TempData["Success"] = "Đã tạo câu hỏi thành công.";
                return RedirectToAction(nameof(Index));
            }

            ApplyErrors(result);
            vm.SubjectOptions = await BuildSubjectOptionsAsync("Chọn môn học", vm.SubjectId);
            return View(vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var question = await _service.GetByIdAsync(id);
            if (question is null)
            {
                return NotFound();
            }

            var vm = ToFormVm(question);
            vm.SubjectOptions = await BuildSubjectOptionsAsync("Chọn môn học", vm.SubjectId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, QuestionFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.SubjectOptions = await BuildSubjectOptionsAsync("Chọn môn học", vm.SubjectId);
                return View(vm);
            }

            var result = await _service.UpdateAsync(id, ToSaveDto(vm));
            if (result.Errors.ContainsKey(ErrorKeys.NotFound))
            {
                return NotFound();
            }

            if (result.IsSuccess)
            {
                TempData["Success"] = "Đã cập nhật câu hỏi thành công.";
                return RedirectToAction(nameof(Index));
            }

            ApplyErrors(result);
            vm.SubjectOptions = await BuildSubjectOptionsAsync("Chọn môn học", vm.SubjectId);
            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var question = await _service.GetByIdAsync(id);
            if (question is null)
            {
                return NotFound();
            }

            return View(ToDetailsVm(question));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _service.GetByIdAsync(id);
            if (question is null)
            {
                return NotFound();
            }

            var result = await _service.DeleteAsync(id);
            if (result.Errors.ContainsKey(ErrorKeys.NotFound))
            {
                return NotFound();
            }

            if (!result.IsSuccess)
            {
                ApplyErrors(result);
                return View(ToDetailsVm(question));
            }

            TempData["Success"] = "Đã xóa câu hỏi thành công.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<SelectList> BuildSubjectOptionsAsync(string emptyLabel, int? selected = null)
        {
            var subjects = await _service.GetSubjectOptionsAsync();
            var items = new List<SelectListItem> { new(emptyLabel, string.Empty) };
            items.AddRange(subjects.Select(s =>
                new SelectListItem($"{s.Code} - {s.Name}", s.SubjectId.ToString())));

            return new SelectList(items, "Value", "Text",
                selected is null ? string.Empty : selected.Value.ToString());
        }

        private static SelectList BuildStatusOptions(QuestionStatus? selected)
        {
            var options = new List<SelectListItem>
            {
                new("Tất cả", string.Empty)
            };

            foreach (QuestionStatus value in Enum.GetValues<QuestionStatus>())
            {
                options.Add(new SelectListItem(GetStatusLabel(value), ((int)value).ToString()));
            }

            return new SelectList(options, "Value", "Text",
                selected is null ? string.Empty : ((int)selected.Value).ToString());
        }

        private static string GetStatusLabel(QuestionStatus value)
        {
            var field = typeof(QuestionStatus).GetField(value.ToString());
            var display = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
                .OfType<DisplayAttribute>()
                .FirstOrDefault();

            return display?.Name ?? value.ToString();
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

        private static string Truncate(string content) =>
            content.Length <= PreviewLength ? content : content[..PreviewLength] + "...";

        private static QuestionSaveDto ToSaveDto(QuestionFormViewModel vm) => new()
        {
            Content = vm.Content,
            SubjectId = vm.SubjectId ?? 0,
            BloomLevel = vm.BloomLevel
        };

        private static QuestionFormViewModel ToFormVm(QuestionDto dto) => new()
        {
            QuestionId = dto.QuestionId,
            Content = dto.Content,
            SubjectId = dto.SubjectId,
            BloomLevel = dto.BloomLevel,
            SubjectCode = dto.SubjectCode,
            SubjectName = dto.SubjectName,
            Status = dto.Status
        };

        private static QuestionDetailsViewModel ToDetailsVm(QuestionDto dto) => new()
        {
            QuestionId = dto.QuestionId,
            Content = dto.Content,
            SubjectId = dto.SubjectId,
            SubjectCode = dto.SubjectCode,
            SubjectName = dto.SubjectName,
            BloomLevel = dto.BloomLevel,
            Status = dto.Status,
            Source = dto.Source,
            CreatedAt = dto.CreatedAt,
            RubricCount = dto.RubricCount,
            RubricCriteria = dto.RubricCriteria
        };
    }
}
