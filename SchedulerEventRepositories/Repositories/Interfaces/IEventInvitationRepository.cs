using SchedulerEventRepositories.Entities;

namespace SchedulerEventRepositories.Repositories.Interfaces;
public interface IEventInvitationRepository
{
    Task CreateEventInvitation(EventInvitation evento);
}