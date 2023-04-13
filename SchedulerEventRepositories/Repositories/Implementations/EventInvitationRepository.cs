using Microsoft.EntityFrameworkCore;
using SchedulerEventCommon.Dtos;
using SchedulerEventCommon.Enums;
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

    public async Task<bool> ExistInvitationByEventIdAndDeveloperId(int eventId, int developerId)
    {
        return await _context.EventInvitations.AnyAsync(e => e.DeveloperId == developerId && e.EventId == eventId && e.State != (int)InvitationStateEnum.Rejected);
    }

    public async Task<EventInvitation> GetInvitationByHash(string hashInvitation)
    {
        return await _context.EventInvitations.FirstOrDefaultAsync(e => e.HasInvitation == hashInvitation);
    }

    public async Task UpdatteInvitation(EventInvitation eventInvitation)
    {
        _context.EventInvitations.Update(eventInvitation);
        await _context.SaveChangesAsync();
    }

    public async Task<List<EventInvitationReportDto>> GetEventInvitationReport(int stateInvitation, int eventId)
    {
        var query = from ei in _context.EventInvitations
                    join e in _context.Events on ei.EventId equals e.Id
                    join d in _context.Developers on ei.DeveloperId equals d.Id
                    where ei.State == stateInvitation && e.Id == eventId
                    orderby d.Name ascending
                    select new EventInvitationReportDto
                    {
                        DeveloperEmail = d.Email,
                        EventName = e.EventName,
                        StateInvitation = ei.State,
                        EventDate = e.EventDate

                    };
                    
        return await query.ToListAsync();
    }
}