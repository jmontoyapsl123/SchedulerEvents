using Microsoft.EntityFrameworkCore;
using SchedulerEventRepositories.DbContext;
using SchedulerEventRepositories.Entities;
using SchedulerEventRepositories.Repositories.Interfaces;

namespace SchedulerEventRepositories.Repositories.Implementations;
public class UserRepository : IUserRepository
{
    private readonly ContextApp _context;
    public UserRepository(ContextApp context) 
    {
        _context = context;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task CreateUser(User user)
    {
         _context.Users.Add(user);
         await _context.SaveChangesAsync();
    }
}