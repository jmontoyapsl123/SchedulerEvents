using Microsoft.EntityFrameworkCore;
using SchedulerEventRepositories.DbContext;
using SchedulerEventRepositories.Entities;
using SchedulerEventRepositories.Repositories.Interfaces;

namespace SchedulerEventRepositories.Repositories.Implementations;
public class EventRepository : IEventRepository
{
    private readonly ContextApp _context;
    public EventRepository(ContextApp context)
    {
        _context = context;
    }

    public async Task CreateEvent(Event evento)
    {
        _context.Events.Add(evento);
        await _context.SaveChangesAsync();
    }

    public async Task<Event> GetEventById(int eventId)
    {
         return await _context.Events.Where(u => u.Id == eventId).FirstOrDefaultAsync();
    }

}