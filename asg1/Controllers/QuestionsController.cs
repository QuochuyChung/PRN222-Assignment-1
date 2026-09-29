using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using asg1.Models.Entities;
using asg1.Models.Repositories;
using asg1.Models.Enums;

namespace asg1.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ISubjectRepository _subjectRepository;

        public QuestionsController(IQuestionRepository questionRepository, ISubjectRepository subjectRepository)
        {
            _questionRepository = questionRepository;
            _subjectRepository = subjectRepository;
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            var questions = await _questionRepository.GetQuestionsWithSubjectAsync();
            return View(questions);
        }

        // GET: Questions/Create
        public async Task<IActionResult> Create()
        {
            var subjects = await _subjectRepository.GetAllAsync();
            ViewBag.SubjectId = new SelectList(subjects, "SubjectId", "Name");
            return View();
        }

        // POST: Questions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubjectId,Content,BloomLevel")] Question question)
        {
            if (ModelState.IsValid)
            {
                question.Status = QuestionStatus.Draft;
                question.Source = QuestionSource.Manual;
                question.CreatedAt = DateTime.UtcNow;
                await _questionRepository.AddAsync(question);
                await _questionRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var subjects = await _subjectRepository.GetAllAsync();
            ViewBag.SubjectId = new SelectList(subjects, "SubjectId", "Name", question.SubjectId);
            return View(question);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var question = await _questionRepository.GetQuestionWithDetailsAsync(id.Value);
            if (question == null) return NotFound();

            var subjects = await _subjectRepository.GetAllAsync();
            ViewBag.SubjectId = new SelectList(subjects, "SubjectId", "Name", question.SubjectId);
            return View(question);
        }

        // POST: Questions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionId,SubjectId,Content,BloomLevel,Status,Source,CreatedAt")] Question question)
        {
            if (id != question.QuestionId) return NotFound();

            if (ModelState.IsValid)
            {
                _questionRepository.Update(question);
                await _questionRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var subjects = await _subjectRepository.GetAllAsync();
            ViewBag.SubjectId = new SelectList(subjects, "SubjectId", "Name", question.SubjectId);
            return View(question);
        }

        // GET: Questions/Review/5
        public async Task<IActionResult> Review(int? id)
        {
            if (id == null) return NotFound();

            var question = await _questionRepository.GetQuestionWithDetailsAsync(id.Value);
            if (question == null) return NotFound();

            return View(question);
        }

        // POST: Questions/Review/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(int id, QuestionStatus status)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null) return NotFound();

            question.Status = status;
            _questionRepository.Update(question);
            await _questionRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var question = await _questionRepository.GetQuestionWithDetailsAsync(id.Value);
            if (question == null) return NotFound();

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question != null)
            {
                _questionRepository.Remove(question);
                await _questionRepository.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
