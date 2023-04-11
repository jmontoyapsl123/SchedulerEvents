using SchedulerEventCommon.Dtos;

namespace SchedulerEventDomain.Services.Interfaces;

public interface IWeatherstackService
{
    Task<WeatherstackDto> GetCityInfo(string city);
}
