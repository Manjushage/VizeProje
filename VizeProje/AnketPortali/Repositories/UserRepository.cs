using AnketPortali.Models;
using Microsoft.EntityFrameworkCore;

namespace AnketPortali.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(AppDbContext context) : base(context, context.Users)
        {
        }
       

    }
}
