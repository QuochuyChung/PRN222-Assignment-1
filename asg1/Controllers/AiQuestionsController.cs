using asg1.Models.AI;
using asg1.Models.Entities;
using asg1.Models.Enums;
using asg1.Models.Repositories;
using asg1.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace asg1.Controllers;

public sealed class AiQuestionsController : Controller
{
    private const long MaxFileSize = 20 * 1024 * 1024;

    private static readonly HashSet<string> SupportedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".doc", ".docx", ".ppt", ".pptx", ".txt", ".md"
    };

    private readonly IAiQuestionGenerator _questionGenerator;
    private readonly ISubjectRepository _subjectRepository;
    private readonly IGenericRepository<Question> _questionRepository;

    public AiQuestionsController(
        IAiQuestionGenerator questionGenerator,
        ISubjectRepository subjectRepository,
        IGenericRepository<Question> questionRepository)
    {
        _questionGenerator = questionGenerator;
        _subjectRepository = subjectRepository;
        _questionRepository = questionRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = new AiQuestionGenerationViewModel();
        await PopulateSubjectsAsync(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        AiQuestionGenerationViewModel model,
        CancellationToken cancellationToken)
    {
        ValidateDocument(model.Document);
        await ValidateSubjectAsync(model.SubjectId);

        if (!ModelState.IsValid)
        {
            await PopulateSubjectsAsync(model);
            return View(model);
        }

        var document = model.Document!;

        try
        {
            await using var stream = document.OpenReadStream();
            model.Questions = await _questionGenerator.GenerateAsync(
                stream,
                document.FileName,
                document.ContentType,
                model.QuestionCount,
                cancellationToken);

            foreach (var generatedQuestion in model.Questions)
            {
                await _questionRepository.AddAsync(new Question
                {
                    SubjectId = model.SubjectId,
                    Content = generatedQuestion.Content,
                    BloomLevel = generatedQuestion.BloomLevel,
                    Source = QuestionSource.AIGenerated,
                    Status = QuestionStatus.Draft
                });
            }

            await _questionRepository.SaveChangesAsync();
            model.SavedQuestionCount = model.Questions.Count;
            model.UploadedFileName = Path.GetFileName(document.FileName);
            model.Document = null;
            await PopulateSubjectsAsync(model);

            return View(model);
        }
        catch (AiQuestionGenerationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            await PopulateSubjectsAsync(model);
            return View(model);
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(
                string.Empty,
                "Đã sinh câu hỏi nhưng không thể lưu vào database. Vui lòng kiểm tra migration và kết nối DB.");
            await PopulateSubjectsAsync(model);
            return View(model);
        }
    }

    private async Task ValidateSubjectAsync(int subjectId)
    {
        if (subjectId <= 0)
        {
            return;
        }

        if (await _subjectRepository.GetByIdAsync(subjectId) is null)
        {
            ModelState.AddModelError(
                nameof(AiQuestionGenerationViewModel.SubjectId),
                "Môn học đã chọn không tồn tại.");
        }
    }

    private async Task PopulateSubjectsAsync(AiQuestionGenerationViewModel model)
    {
        var subjects = await _subjectRepository.GetAllAsync();
        model.Subjects = subjects
            .OrderBy(subject => subject.Code)
            .Select(subject => new SelectListItem(
                $"{subject.Code} - {subject.Name}",
                subject.SubjectId.ToString(),
                subject.SubjectId == model.SubjectId))
            .ToList();
    }

    private void ValidateDocument(IFormFile? document)
    {
        if (document is null || document.Length == 0)
        {
            ModelState.AddModelError(nameof(AiQuestionGenerationViewModel.Document), "Vui lòng chọn tài liệu có nội dung.");
            return;
        }

        if (document.Length > MaxFileSize)
        {
            ModelState.AddModelError(nameof(AiQuestionGenerationViewModel.Document), "Tài liệu không được vượt quá 20 MB.");
        }

        var extension = Path.GetExtension(document.FileName);
        if (!SupportedExtensions.Contains(extension))
        {
            ModelState.AddModelError(
                nameof(AiQuestionGenerationViewModel.Document),
                "Chỉ hỗ trợ PDF, Word, PowerPoint, TXT và Markdown.");
        }
    }
}
