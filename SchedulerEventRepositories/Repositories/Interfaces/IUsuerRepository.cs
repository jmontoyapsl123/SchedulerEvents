using SchedulerEventRepositories.Entities;

namespace SchedulerEventRepositories.Repositories.Interfaces;
public interface IUserRepository
{
    Task<User> GetUserByEmail(string email);
    Task CreateUser(User user);
}