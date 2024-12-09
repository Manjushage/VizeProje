using AnketPortali.Models;
using AnketPortali.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AnketPortali.Repositories
{
    public class QuestionRepository : GenericRepository<Question>
    {
        public QuestionRepository(AppDbContext context) : base(context, context.Questions)
        {
        }
        public async Task<List<Question>> GetQuestionsBySurveyIdAsync(int surveyId)
        {
            return await _context.Questions
                .Where(q => q.SurveyId == surveyId)
                .Include(q => q.Answers) // Gerekirse yanıtları da ekleyebilirsiniz
                .ToListAsync();
        }
      

        // Anketin tüm sorularını ve cevaplarını almak için başka bir method ekleyebilirsiniz
        public async Task<Question> GetQuestionWithAnswersAsync(int questionId)
        {
            return await _context.Questions
                .Where(q => q.Id == questionId)
                .Include(q => q.Answers)  // Yanıtları da dahil et
                .FirstOrDefaultAsync(); // Tek bir soru al
        }
        public async Task<bool> HasUserAnsweredSurveyAsync(int surveyId, int userId)
        {
            return await _context.SurveyApplications
                .AnyAsync(sa => sa.SurveyId == surveyId && sa.UserId == userId);
        }
    }
}
