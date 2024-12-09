using AnketPortali.Models;
using AnketPortali.Repositories;

namespace AnketPortali.Repositories
{
    public class AnswersRepository : GenericRepository<Answer>
    {
        public AnswersRepository(AppDbContext context) : base(context,context.Answers)
        {
        }
    }
}
