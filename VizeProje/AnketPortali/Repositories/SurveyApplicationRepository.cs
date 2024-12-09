using AnketPortali.Models;
using AnketPortali.Models;
using AnketPortali.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AnketPortali.Repositories
{
    public class SurveyApplicationRepository : GenericRepository<SurveyApplication>
    {
        public SurveyApplicationRepository(AppDbContext contex) : base(contex,contex.SurveyApplications)
        {
        }
        public async Task<SurveyApplication> GetAsync(Expression<Func<SurveyApplication, bool>> predicate)
        {
            return await _context.SurveyApplications.FirstOrDefaultAsync(predicate);
        }
    }
}
