using FluentValidation;
using Microsoft.Extensions.Configuration;
using ScedulerEventDomain.Services.Interfaces;
using SchedulerEventCommon.Dtos;
using SchedulerEventCommon.Enums;
using SchedulerEventDomain.Services.Interfaces;
using SchedulerEventRepositories.Entities;
using SchedulerEventRepositories.Repositories.Interfaces;

namespace ScedulerEventDomain.Services.Implementations;
public class EventInvitationService : IEventInvitationService
{
    private readonly IEventInvitationRepository _eventInvitationRepository;
    private readonly IValidator<EventInvitationDto> _eventInvitationValidator;
    private readonly IValidator<ReportParamDto> _reportParameterValidator;
    private readonly IConfiguration _configuration;
    private readonly IDeveloperRepository _developerRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ISendEmailService _sendService;
    public EventInvitationService(IEventInvitationRepository eventInvitationRepository, IValidator<EventInvitationDto> eventInvitationValidator, IConfiguration configuration,
    IDeveloperRepository developerRepository, IEventRepository eventRepository, ISendEmailService sendService, IValidator<ReportParamDto> reportParameterValidator)
    {
        _eventInvitationRepository = eventInvitationRepository;
        _eventInvitationValidator = eventInvitationValidator;
        _configuration = configuration;
        _developerRepository = developerRepository;
        _eventRepository = eventRepository;
        _sendService = sendService;
        _reportParameterValidator = reportParameterValidator;
    }

    public async Task<ResponseDto<bool>> AcceptEventInvitation(string hashInvitation)
    {
        var invitation = await _eventInvitationRepository.GetInvitationByHash(hashInvitation);
        ResponseDto<bool> response = new ResponseDto<bool>();
        response.HasError = true;
        if (invitation == null)
        {
            response.Errors = $"Invalid link.";
            return response;
        }

        invitation.State = (int)InvitationStateEnum.Accepted;
        invitation.HasInvitation = null;
        await _eventInvitationRepository.UpdatteInvitation(invitation);
        response.HasError = false;
        response.Result = true;
        return response;

    }

    public async Task<ResponseDto<bool>> RejectEventInvitation(string hashInvitation)
    {
        var invitation = await _eventInvitationRepository.GetInvitationByHash(hashInvitation);
        ResponseDto<bool> response = new ResponseDto<bool>();
        response.HasError = true;
        if (invitation == null)
        {
            response.Errors = $"Invalid link.";
            return response;
        }

        invitation.State = (int)InvitationStateEnum.Rejected;
        invitation.HasInvitation = null;
        await _eventInvitationRepository.UpdatteInvitation(invitation);
        response.HasError = false;
        response.Result = true;
        return response;
    }

    public async Task<ResponseDto<int>> CreateEventInvitation(EventInvitationDto eventInvitationDto)
    {
        var validation = await _eventInvitationValidator.ValidateAsync(eventInvitationDto);
        ResponseDto<int> response = new ResponseDto<int>();
        response.HasError = true;
        if (!validation.IsValid)
        {
            response.Errors = string.Join(", ", validation.Errors);
            return response;
        }

        //no se haya invitado ya al developer
        var existInvitation = await _eventInvitationRepository.ExistInvitationByEventIdAndDeveloperId(eventInvitationDto.EventId, eventInvitationDto.DeveloperId);
        if (existInvitation)
        {
            response.Errors = $"There is a invitation created for the developer.";
            return response;
        }

        var developer = await _developerRepository.GetDeveloperById(eventInvitationDto.DeveloperId);
        if (developer == null)
        {
            response.Errors = $"The developer does not exist.";
            return response;
        }

        var eventbd = await _eventRepository.GetEventById(eventInvitationDto.EventId);
        if (eventbd == null)
        {
            response.Errors = $"The event does not exist.";
            return response;
        }

        EventInvitation newEventInvitation = new EventInvitation
        {
            DeveloperId = eventInvitationDto.DeveloperId,
            EventId = eventInvitationDto.EventId,
            State = (int)InvitationStateEnum.Pending,
            HasInvitation = Guid.NewGuid().ToString()
        };

        await _eventInvitationRepository.CreateEventInvitation(newEventInvitation);
        var urlBack = _configuration.GetSection("UrlBack").Value;
        var urlAccept = @$"{urlBack}{_configuration.GetSection("AcceptInvitationEndpoint").Value}?hash={newEventInvitation.HasInvitation}";
        var urlReject = @$"{urlBack}{_configuration.GetSection("RejectInvitationEndpoint").Value}?hash={newEventInvitation.HasInvitation}";
        var message = $"You are Invitated to the event: {eventbd.EventName} to accept the invitation please click <a href=\"{urlAccept}\">here</a> or reject it click <a href=\"{urlReject}\">here</a>";

        await _sendService.SendEmailAsync(developer.Email, message);
        response.HasError = false;
        response.Result = newEventInvitation.Id;
        return response;
    }

    public async Task<ResponseDto<List<EventInvitationReportDto>>> GetEventInvitationReport(ReportParamDto parameters)
    {
        var validation = await _reportParameterValidator.ValidateAsync(parameters);
        ResponseDto<List<EventInvitationReportDto>> response = new ResponseDto<List<EventInvitationReportDto>>();
        response.HasError = true;
        if (!validation.IsValid)
        {
            response.Errors = string.Join(", ", validation.Errors);
            return response;
        }
        
        response.HasError = false;
        response.Result = await _eventInvitationRepository.GetEventInvitationReport(parameters.StateInvitation, parameters.EventId);
        return response;
    }
}