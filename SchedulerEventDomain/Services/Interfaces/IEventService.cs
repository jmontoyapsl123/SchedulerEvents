using SchedulerEventCommon.Dtos;

namespace ScedulerEventDomain.Services.Interfaces;
public interface IEventService
{
    Task<ResponseDto<int>> CreateEvent(EventDto eventDto);
}