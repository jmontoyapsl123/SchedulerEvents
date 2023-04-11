using SchedulerEventRepositories.Entities;

namespace SchedulerEventRepositories.Repositories.Interfaces;
public interface IEventRepository
{
    Task CreateEvent(Event evento);
}