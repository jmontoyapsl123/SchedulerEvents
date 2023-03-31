using SchedulerEventRepositories.DbContext;
using SchedulerEventRepositories.Entities;

namespace SchedulerEventRepositories.Repositories;
public class DeveloperRepository : IDeveloperRepository
{
    private static Context _context;
    public DeveloperRepository(Context context)
    {
        _context = context;
    }

    public async Task CreateDeveloper(Developer developer)
    {
         _context.Developers.Add(developer);
         await _context.SaveChangesAsync();
    }
}