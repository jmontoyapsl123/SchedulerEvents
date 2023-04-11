using FluentValidation;
using ScedulerEventDomain.Services.Interfaces;
using SchedulerEventCommon.Dtos;
using SchedulerEventCommon.Enums;
using SchedulerEventRepositories.Entities;
using SchedulerEventRepositories.Repositories.Interfaces;

namespace ScedulerEventDomain.Services.Implementations;
public class EventInvitationService : IEventInvitationService
{
    private static IEventInvitationRepository _eventInvitationRepository;
    private readonly IValidator<EventInvitationDto> _eventInvitationValidator;
    public EventInvitationService(IEventInvitationRepository eventInvitationRepository, IValidator<EventInvitationDto> eventInvitationValidator)
    {
        _eventInvitationRepository = eventInvitationRepository;
        _eventInvitationValidator = eventInvitationValidator;
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

        EventInvitation newEventInvitation = new EventInvitation
        {
            DeveloperId = eventInvitationDto.DeveloperId,
            EventId = eventInvitationDto.EventId,
            State = (int)InvitationStateEnum.Pending 
        };

        await _eventInvitationRepository.CreateEventInvitation(newEventInvitation);
        response.HasError = false;
        response.Result = newEventInvitation.Id;
        return response;
    }
}