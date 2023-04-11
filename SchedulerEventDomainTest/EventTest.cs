using FluentValidation;
using FluentValidation.Results;
using Moq;
using ScedulerEventDomain.Services.Implementations;
using ScedulerEventDomain.Services.Interfaces;
using SchedulerEventCommon.Dtos;
using SchedulerEventCommon.Enums;
using SchedulerEventDomain.Services.Interfaces;
using SchedulerEventRepositories.Entities;
using SchedulerEventRepositories.Repositories.Interfaces;

namespace SchedulerEventDomainTest;
public class EventTest
{
    private readonly Mock<IEventRepository> _eventRepository;
    private readonly Mock<IValidator<EventDto>> _eventValidator;
    private readonly Mock<IWeatherstackService> _weatherstackService;
    private readonly IEventService _eventService;
    private readonly EventDto _eventDto;
    public EventTest()
    {
        _eventRepository = new Mock<IEventRepository>();
        _eventValidator = new Mock<IValidator<EventDto>>(MockBehavior.Strict);
        _weatherstackService = new Mock<IWeatherstackService>();
        _eventService = new EventService(_eventRepository.Object, _eventValidator.Object, _weatherstackService.Object);
        _eventDto = new EventDto
        {
            EventName = "Event Name",
            Description = "Description Name",
            EventDate = DateTime.Now,
            EventType = (int)EventTypeEnum.Virtual,
            City = "New York"
        };
    }
    [Fact]
    public async void Create_Event_Virtual_Type_Should_Save()
    {
        //Arrange
        Event newEvent = null;
        _eventRepository.Setup(r => r.CreateEvent(It.IsAny<Event>()))
            .Callback<Event>(x => newEvent = x);
        _eventValidator.Setup(validator => validator.ValidateAsync(_eventDto, It.IsAny<CancellationToken>()))
       .ReturnsAsync(new ValidationResult());
        var result = await _eventService.CreateEvent(_eventDto);
        _eventRepository.Verify(x => x.CreateEvent(It.IsAny<Event>()), Times.Once);
        _eventValidator.Verify(x => x.ValidateAsync(_eventDto, It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(_eventDto.EventName, newEvent.EventName);
    }

    [Fact]
    public async void Create_Event_InPerson_Type_Should_Save()
    {
        //Arrange
        Event newEvent = null;
        _eventDto.EventType = (int)EventTypeEnum.InPerson;
        _eventRepository.Setup(r => r.CreateEvent(It.IsAny<Event>()))
            .Callback<Event>(x => newEvent = x);
        var weatherstackResult = new WeatherstackDto
        {
            Location = new Location
            {
                Country = "United States of America",
                Lat = "40.714",
                Lon = "-74.006",
                Name = "New York"
            },
            Request = new Request
            {
                Language = "en"
            }
        };

        _weatherstackService.Setup(r => r.GetCityInfo(It.IsAny<string>())).ReturnsAsync(weatherstackResult);
        _eventValidator.Setup(validator => validator.ValidateAsync(_eventDto, It.IsAny<CancellationToken>()))
       .ReturnsAsync(new ValidationResult());
        var result = await _eventService.CreateEvent(_eventDto);
        _eventRepository.Verify(x => x.CreateEvent(It.IsAny<Event>()), Times.Once);
        _weatherstackService.Verify(x => x.GetCityInfo(It.IsAny<string>()), Times.AtLeastOnce);
        _eventValidator.Verify(x => x.ValidateAsync(_eventDto, It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(_eventDto.EventName, newEvent.EventName);
        Assert.Equal("en", newEvent.Lenguage);
        Assert.Equal("40.714", newEvent.Latitude);
        Assert.Equal("-74.006", newEvent.Longitude);
        Assert.Equal("United States of America", newEvent.Country);
    }
}