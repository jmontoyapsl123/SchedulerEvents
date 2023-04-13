using SchedulerEventCommon.Dtos;
using SchedulerEventRepositories.Entities;

namespace SchedulerEventRepositories.Repositories.Interfaces;
public interface IEventInvitationRepository
{
    Task CreateEventInvitation(EventInvitation evento);
    Task<bool> ExistInvitationByEventIdAndDeveloperId(int eventId, int developerId);
    Task<EventInvitation>GetInvitationByHash(string hashInvitation);
    Task UpdatteInvitation(EventInvitation eventInvitation);
    Task<List<EventInvitationReportDto>> GetEventInvitationReport(int stateInvitation, int eventId);
}