using AnketPortali.Repositories;
using AnketPortali.ViewModel;
using AutoMapper;
using AnketPortali.Models;
using AnketPortali.Repositories;
using AnketPortali.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace AnketPortali.Controllers
{
    public class SurveyController : Controller
    {
        private readonly SurveyRepository _surveyRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly QuestionRepository _questionRepository;
        private readonly AnswersRepository _answersRepository;
        private readonly SurveyApplicationRepository _surveyApplicationRepository;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyfService;

        public SurveyController(SurveyRepository surveyRepository, CategoryRepository categoryRepository, QuestionRepository questionRepository, AnswersRepository answersRepository, SurveyApplicationRepository surveyApplicationRepository, IMapper mapper, INotyfService notyfService)
        {
            _surveyRepository = surveyRepository;
            _categoryRepository = categoryRepository;
            _questionRepository = questionRepository;
            _answersRepository = answersRepository;
            _surveyApplicationRepository = surveyApplicationRepository;
            _mapper = mapper;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> Index()
        {
            var surveys = await _surveyRepository.GetAllSurvey();
            return View(surveys);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SurveyModel model)
        {
            if (ModelState.IsValid)
            {
                var survey = _mapper.Map<Survey>(model);
                survey.Created = DateTime.Now;
                survey.Updated = DateTime.Now;
                await _surveyRepository.AddAsync(survey);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var survey = await _surveyRepository.GetByIdAsync(id);
            if (survey == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(survey);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SurveyModel model)
        {
            if (ModelState.IsValid)
            {
                var survey = _mapper.Map<Survey>(model);
                await _surveyRepository.UpdateAsync(survey);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _surveyRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AddQuestion(int surveyId)
        {
            // Var olan sorular� getiriyoruz
            var questions = await _questionRepository.GetQuestionsBySurveyIdAsync(surveyId);

            var model = new AddQuestionViewModel
            {
                SurveyId = surveyId,
                Questions = questions.Select(q => new QuestionViewModel
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText
                }).ToList()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddQuestion(AddQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Yeni soru ekliyoruz
                var question = new Question
                {
                    QuestionText = model.QuestionText,
                    SurveyId = model.SurveyId,
                    Answers = Enum.GetValues(typeof(AnswerType))
                                  .Cast<AnswerType>()
                                  .Select(a => new Answer { AnswerValue = a })
                                  .ToList()
                };

                await _questionRepository.AddAsync(question);
            }

            // Yeniden sorular� getirip sayfaya g�nderiyoruz
            var questions = await _questionRepository.GetQuestionsBySurveyIdAsync(model.SurveyId);

            model.Questions = questions.Select(q => new QuestionViewModel
            {
                Id = q.Id,
                QuestionText = q.QuestionText
            }).ToList();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteQuestion(int questionId, int surveyId)
        {
            // Soruyu siliyoruz
            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question != null)
            {
                await _questionRepository.DeleteAsync(question.Id);
            }

            // Silme i�leminden sonra formu yeniden y�klemek i�in gerekli sorular� al�yoruz
            var questions = await _questionRepository.GetQuestionsBySurveyIdAsync(surveyId);

            var model = new AddQuestionViewModel
            {
                SurveyId = surveyId,
                Questions = questions.Select(q => new QuestionViewModel
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText
                }).ToList()
            };

            return View("AddQuestion", model); // Ayn� sayfay� y�kle
        }
        [Authorize]
        public async Task<IActionResult> TakeSurvey(int surveyId)
        {
            var usersIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Kullanıcı ID'si string olarak alınıyor


            if (int.TryParse(usersIdString, out int usersId))
            {
                // Dönüşüm başarılı, usersId'yi kullanabilirsiniz
                var hasAnswered = await _questionRepository.HasUserAnsweredSurveyAsync(surveyId, usersId);
                if (hasAnswered)
                {
                    TempData["ErrorMessage"] = "Bu ankete zaten cevap verdiniz.";
                    return RedirectToAction("Index", "Home"); // Veya başka bir sayfaya yönlendirme
                }
            }

            var questions = await _questionRepository.GetQuestionsBySurveyIdAsync(surveyId);

            if (questions == null || !questions.Any())
            {
                TempData["ErrorMessage"] = "Anket soruları bulunamadı, anket hazırlama aşamasında.";
                return RedirectToAction("Index", "Home"); // Ana sayfaya veya başka bir sayfaya yönlendirme
            }
            ViewBag.SurveyId = surveyId; // Anket ID'sini View'a gönder
            return View(questions); // Sorular� View'a g�nder
        }

        // Cevaplar� kaydetme
        [HttpPost]
        public async Task<IActionResult> SubmitAnswers(Dictionary<int, Answer> answers,int Surveyids)
        {
            var usersIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Kullanıcı ID'si string olarak alınıyor


            if (int.TryParse(usersIdString, out int usersId))
            {
                var added= _surveyApplicationRepository.AddAsync(new SurveyApplication
                {
                    SurveyId = Surveyids,
                    UserId = usersId,
                    AppliedDate = DateTime.Now
                });
                
            }



            if (answers == null || !answers.Any())
            {
                return BadRequest("Ge�erli cevaplar girilmelidir.");
            }

            // Cevaplar� veritaban�na kaydet
            foreach (var answer in answers)
            {

                // Yeni Answer nesnesi olu�tur
                var newAnswer = new Answer
                {
                    QuestionId = answer.Key,
                    AnswerValue = answer.Value.AnswerValue,
                };

                await _answersRepository.AddAsync(newAnswer);
            }

           
            _notyfService.Success("Anketi başarıyla yanıtladınız.", 5);
            return RedirectToAction("Index", "Home"); // Ba�ka bir sayfaya y�nlendirme

        }


    }
} 