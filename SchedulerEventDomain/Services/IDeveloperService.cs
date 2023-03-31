using SchedulerEventCommon.Dtos;

namespace ScedulerEventDomain.Services;
public interface IDeveloperService
{
    Task<int> CreateDeveloper(DeveloperDto developerDto);
}