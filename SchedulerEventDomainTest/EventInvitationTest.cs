using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Moq;
using ScedulerEventDomain.Services.Implementations;
using SchedulerEventCommon.Dtos;
using SchedulerEventCommon.Enums;
using SchedulerEventDomain.Services.Interfaces;
using SchedulerEventRepositories.Entities;
using SchedulerEventRepositories.Repositories.Interfaces;

namespace SchedulerEventDomainTest;
public class EventInvitationTest
{
    private readonly Mock<IEventRepository> _eventRepository;
    private readonly Mock<IValidator<EventInvitationDto>> _eventInvitationValidator;
    private readonly Mock<IValidator<ReportParamDto>> _reportParameterValidator;
    private readonly Mock<IEventInvitationRepository> _eventInvitationRepository;
    private readonly Mock<IDeveloperRepository> _developerRepository;
    private readonly Mock<IConfiguration> _configuration;
    private readonly EventInvitationService _eventInvitationService;
    private readonly Mock<ISendEmailService> _sendService;
    private readonly EventInvitationDto _eventInvitationDto;
    public EventInvitationTest()
    {
        _eventRepository = new Mock<IEventRepository>();
        _eventInvitationValidator = new Mock<IValidator<EventInvitationDto>>(MockBehavior.Strict);
        _reportParameterValidator = new Mock<IValidator<ReportParamDto>>(MockBehavior.Strict);
        _configuration = new Mock<IConfiguration>();
        _developerRepository = new Mock<IDeveloperRepository>();
        _eventInvitationRepository = new Mock<IEventInvitationRepository>();
        _sendService = new Mock<ISendEmailService>();
        _eventInvitationService = new EventInvitationService(_eventInvitationRepository.Object, _eventInvitationValidator.Object, _configuration.Object, _developerRepository.Object,
        _eventRepository.Object, _sendService.Object, _reportParameterValidator.Object);
        _eventInvitationDto = new EventInvitationDto
        {
            EventId = 1,
            DeveloperId = 2
        };
    }

    [Fact]
    public async void Create_Event_Invitation_Should_Save()
    {
        //Arrange
        EventInvitation newEventInvitation = null;
        _eventInvitationRepository.Setup(r => r.CreateEventInvitation(It.IsAny<EventInvitation>()))
            .Callback<EventInvitation>(x => newEventInvitation = x);
        _eventInvitationValidator.Setup(validator => validator.ValidateAsync(_eventInvitationDto, It.IsAny<CancellationToken>()))
       .ReturnsAsync(new ValidationResult());
        _eventInvitationRepository.Setup(r => r.ExistInvitationByEventIdAndDeveloperId(It.IsAny<int>(), It.IsAny<int>()))
        .ReturnsAsync(false);
        _developerRepository.Setup(r => r.GetDeveloperById(It.IsAny<int>()))
       .ReturnsAsync(new Developer());
        _eventRepository.Setup(r => r.GetEventById(It.IsAny<int>()))
        .ReturnsAsync(new Event());
        _configuration.Setup(repo => repo.GetSection(It.IsAny<string>()).Value).Returns("SomeText");
        _sendService.Setup(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
        .Returns(Task.CompletedTask);

        var result = await _eventInvitationService.CreateEventInvitation(_eventInvitationDto);

        _eventInvitationRepository.Verify(x => x.CreateEventInvitation(It.IsAny<EventInvitation>()), Times.Once);
        _eventInvitationValidator.Verify(x => x.ValidateAsync(_eventInvitationDto, It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(_eventInvitationDto.DeveloperId, newEventInvitation.DeveloperId);
        Assert.Equal(_eventInvitationDto.EventId, newEventInvitation.EventId);
        Assert.Equal((int)InvitationStateEnum.Pending, newEventInvitation.State);
        Assert.NotEmpty(newEventInvitation.HasInvitation);
    }

    [Fact]
    public async void Create_Event_Invitation_Should_Not_Save_Exist_Invitation()
    {
        //Arrange
        _eventInvitationValidator.Setup(validator => validator.ValidateAsync(_eventInvitationDto, It.IsAny<CancellationToken>()))
         .ReturnsAsync(new ValidationResult());
        _eventInvitationRepository.Setup(r => r.ExistInvitationByEventIdAndDeveloperId(It.IsAny<int>(), It.IsAny<int>()))
               .ReturnsAsync(true);
        var result = await _eventInvitationService.CreateEventInvitation(_eventInvitationDto);
        Assert.True(result.HasError);
        Assert.Equal("There is a invitation created for the developer.", result.Errors);
    }

    [Fact]
    public async void Create_Event_Invitation_Should_Not_Save_Developer_Does_Not()
    {
        //Arrange
        _eventInvitationValidator.Setup(validator => validator.ValidateAsync(_eventInvitationDto, It.IsAny<CancellationToken>()))
         .ReturnsAsync(new ValidationResult());
        _eventInvitationRepository.Setup(r => r.ExistInvitationByEventIdAndDeveloperId(It.IsAny<int>(), It.IsAny<int>()))
               .ReturnsAsync(false);
        _developerRepository.Setup(r => r.GetDeveloperById(It.IsAny<int>()))
       .ReturnsAsync(() => null);
        var result = await _eventInvitationService.CreateEventInvitation(_eventInvitationDto);
        Assert.True(result.HasError);
        Assert.Equal("The developer does not exist.", result.Errors);
    }

    [Fact]
    public async void Create_Event_Invitation_Should_Not_Save_Event_Does_Not()
    {
        //Arrange
        _eventInvitationValidator.Setup(validator => validator.ValidateAsync(_eventInvitationDto, It.IsAny<CancellationToken>()))
         .ReturnsAsync(new ValidationResult());
        _eventInvitationRepository.Setup(r => r.ExistInvitationByEventIdAndDeveloperId(It.IsAny<int>(), It.IsAny<int>()))
               .ReturnsAsync(false);
        _developerRepository.Setup(r => r.GetDeveloperById(It.IsAny<int>()))
       .ReturnsAsync(new Developer());
        _eventRepository.Setup(r => r.GetEventById(It.IsAny<int>()))
        .ReturnsAsync(() => null);
        var result = await _eventInvitationService.CreateEventInvitation(_eventInvitationDto);
        Assert.True(result.HasError);
        Assert.Equal("The event does not exist.", result.Errors);
    }

    [Fact]
    public async void Accept_Event_Invitation_Should_Not_Save()
    {
        //Arrange
        _eventInvitationRepository.Setup(r => r.GetInvitationByHash(It.IsAny<string>()))
               .ReturnsAsync(() => null);
        var result = await _eventInvitationService.AcceptEventInvitation("AnyText");
        Assert.True(result.HasError);
        Assert.Equal("Invalid link.", result.Errors);
    }

    [Fact]
    public async void Reject_Event_Invitation_Should_Not_Save()
    {
        //Arrange
        _eventInvitationRepository.Setup(r => r.GetInvitationByHash(It.IsAny<string>()))
               .ReturnsAsync(() => null);
        var result = await _eventInvitationService.RejectEventInvitation("AnyText");
        Assert.True(result.HasError);
        Assert.Equal("Invalid link.", result.Errors);
    }

    [Fact]
    public async void Accept_Event_Invitation_Should_Save()
    {
        var invitation = new EventInvitation
        {
            DeveloperId = 1,
            EventId = 2,
            State = 0
        };
        //Arrange
        _eventInvitationRepository.Setup(r => r.GetInvitationByHash(It.IsAny<string>()))
               .ReturnsAsync(() => invitation);
        var result = await _eventInvitationService.AcceptEventInvitation("AnyText");
        Assert.False(result.HasError);
        Assert.Equal(invitation.State, 1);
    }

    [Fact]
    public async void Reject_Event_Invitation_Should_Save()
    {
        var invitation = new EventInvitation
        {
            DeveloperId = 1,
            EventId = 2,
            State = 0
        };
        //Arrange
        _eventInvitationRepository.Setup(r => r.GetInvitationByHash(It.IsAny<string>()))
               .ReturnsAsync(() => invitation);
        var result = await _eventInvitationService.RejectEventInvitation("AnyText");
        Assert.False(result.HasError);
        Assert.Equal(invitation.State, 2);
    }

    [Fact]
    public async void Get_Event_Invitation_Report_Should_Fail()
    {
        //Arrange
        var resultValidation = new ValidationResult();
        var _parameterDto = new ReportParamDto();
        resultValidation.Errors.Add(new ValidationFailure("EventId", "'Event Id' must be greater than '0'."));
        _reportParameterValidator.Setup(validator => validator.ValidateAsync(_parameterDto, It.IsAny<CancellationToken>()))
        .ReturnsAsync(resultValidation);

        var result = await _eventInvitationService.GetEventInvitationReport(_parameterDto);
        Assert.True(result.HasError);
        Assert.Equal("'Event Id' must be greater than '0'.", result.Errors);

    }

    [Fact]
    public async void Get_Event_Invitation_Report_Should_Result()
    {
        var _parameterDto = new ReportParamDto();
        _reportParameterValidator.Setup(validator => validator.ValidateAsync(_parameterDto, It.IsAny<CancellationToken>()))
         .ReturnsAsync(new ValidationResult());
        List<EventInvitationReportDto> eventInvitationReportDtos = new List<EventInvitationReportDto>();
        eventInvitationReportDtos.Add(new EventInvitationReportDto
        {
            DeveloperEmail = "test@test.com",
            EventDate = new DateTime(),
            EventName = "Evento 1",
            StateInvitation = 1
        });
        
          _eventInvitationRepository.Setup(r => r.GetEventInvitationReport(It.IsAny<int>(), It.IsAny<int>()))
        .ReturnsAsync(eventInvitationReportDtos);
        var result = await _eventInvitationService.GetEventInvitationReport(_parameterDto);
        _eventInvitationRepository.Verify(r => r.GetEventInvitationReport(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        _reportParameterValidator.Verify(x => x.ValidateAsync(_parameterDto, It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(eventInvitationReportDtos.SequenceEqual(result.Result));
    }
}