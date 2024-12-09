using AnketPortali.Models;
using AnketPortali.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AnketPortali.Repositories
{
    public class SurveyRepository : GenericRepository<Survey>
    {
        public SurveyRepository(AppDbContext context) : base(context, context.Surveys)
        {
        }
        
        public async Task<IEnumerable<Survey>> GetAllSurvey()
        {
            return await _context.Surveys.Include(x => x.Category).ToListAsync();
        }
        public async Task<Question> GetByIdAsync(int questionId)
        {
            return await _context.Questions.FirstOrDefaultAsync(q => q.Id == questionId);
        }
    }
}
