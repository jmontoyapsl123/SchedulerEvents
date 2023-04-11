using SchedulerEventCommon.Dtos;

namespace ScedulerEventDomain.Services.Interfaces;
public interface IEventInvitationService
{
    Task<ResponseDto<int>> CreateEventInvitation(EventInvitationDto eventInvitationDto);
}