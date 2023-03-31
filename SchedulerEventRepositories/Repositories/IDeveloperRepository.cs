using SchedulerEventRepositories.Entities;

namespace SchedulerEventRepositories.Repositories;
public interface IDeveloperRepository  {
    Task CreateDeveloper(Developer developer);
}