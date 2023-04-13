using FluentValidation;
using ScedulerEventDomain.Services.Interfaces;
using SchedulerEventCommon.Dtos;
using SchedulerEventCommon.Enums;
using SchedulerEventDomain.Services.Interfaces;
using SchedulerEventRepositories.Entities;
using SchedulerEventRepositories.Repositories.Interfaces;

namespace ScedulerEventDomain.Services.Implementations;
public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IValidator<EventDto> _eventValidator;
    private readonly IWeatherstackService _weatherstackService;
    public EventService(IEventRepository eventRepository, IValidator<EventDto> eventValidator, IWeatherstackService weatherstackService)
    {
        _eventRepository = eventRepository;
        _eventValidator = eventValidator;
        _weatherstackService = weatherstackService;
    }

    public async Task<ResponseDto<int>> CreateEvent(EventDto eventDto)
    {
        var validation = await _eventValidator.ValidateAsync(eventDto);
        ResponseDto<int> response = new ResponseDto<int>();
        response.HasError = true;
        if (!validation.IsValid)
        {
            response.Errors = string.Join(", ", validation.Errors);
            return response;
        }

        Event newEvent = new Event
        {
            City = eventDto.City,
            Description = eventDto.Description,
            EventType = eventDto.EventType,
            EventDate = eventDto.EventDate,
            EventName = eventDto.EventName
        };

        if (eventDto.EventType == (int)EventTypeEnum.InPerson)
        {
            var weatherstackInfo = await _weatherstackService.GetCityInfo(eventDto.City);
            newEvent.Country = weatherstackInfo.Location.Country;
            newEvent.Latitude = weatherstackInfo.Location.Lat;
            newEvent.Longitude = weatherstackInfo.Location.Lon;
            newEvent.Lenguage = weatherstackInfo.Request.Language;
        }

        await _eventRepository.CreateEvent(newEvent);
        response.HasError = false;
        response.Result = newEvent.Id;
        return response;
    }
}