using SchedulerEventCommon.Dtos;

namespace ScedulerEventDomain.Services.Interfaces;
public interface IDeveloperService
{
    Task<ResponseDto<int>> CreateDeveloper(DeveloperDto developerDto);
}