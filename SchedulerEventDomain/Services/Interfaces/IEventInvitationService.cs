using SchedulerEventCommon.Dtos;

namespace ScedulerEventDomain.Services.Interfaces;
public interface IEventInvitationService
{
    Task<ResponseDto<int>> CreateEventInvitation(EventInvitationDto eventInvitationDto);
    Task<ResponseDto<bool>> RejectEventInvitation(string hashInvitation);
    Task<ResponseDto<bool>> AcceptEventInvitation(string hashInvitation);
    Task<ResponseDto<List<EventInvitationReportDto>>> GetEventInvitationReport(ReportParamDto parameters);
}