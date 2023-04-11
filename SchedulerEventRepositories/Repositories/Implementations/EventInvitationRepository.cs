using SchedulerEventRepositories.DbContext;
using SchedulerEventRepositories.Entities;
using SchedulerEventRepositories.Repositories.Interfaces;

namespace SchedulerEventRepositories.Repositories.Implementations;
public class EventInvitationRepository : IEventInvitationRepository
{
    private readonly ContextApp _context;
    public EventInvitationRepository(ContextApp context)
    {
        _context = context;
    }

    public async Task CreateEventInvitation(EventInvitation eventInvitation)
    {
        _context.EventInvitations.Add(eventInvitation);
        await _context.SaveChangesAsync();
    }


}