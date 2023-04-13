using Microsoft.EntityFrameworkCore;
using SchedulerEventRepositories.DbContext;
using SchedulerEventRepositories.Entities;
using SchedulerEventRepositories.Repositories.Interfaces;

namespace SchedulerEventRepositories.Repositories.Implementations;
public class DeveloperRepository : IDeveloperRepository
{
    private readonly ContextApp _context;
    public DeveloperRepository(ContextApp context)
    {
        _context = context;
    }

    public async Task CreateDeveloper(Developer developer)
    {
         _context.Developers.Add(developer);
         await _context.SaveChangesAsync();
    }

    public async Task<Developer> GetDeveloperByEmail(string email)
    {
         return await _context.Developers.Where(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task<Developer> GetDeveloperById(int developerId)
    {
         return await _context.Developers.Where(u => u.Id == developerId).FirstOrDefaultAsync();
    }
}