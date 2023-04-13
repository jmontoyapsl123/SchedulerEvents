using SchedulerEventRepositories.Entities;

namespace SchedulerEventRepositories.Repositories.Interfaces;
public interface IDeveloperRepository  {
    Task CreateDeveloper(Developer developer);
    Task<Developer> GetDeveloperByEmail(string email);
    Task<Developer> GetDeveloperById(int developerId);
}