using FluentValidation;
using FluentValidation.Results;
using Moq;
using ScedulerEventDomain.Services.Implementations;
using ScedulerEventDomain.Services.Interfaces;
using SchedulerEventCommon.Dtos;
using SchedulerEventRepositories.Entities;
using SchedulerEventRepositories.Repositories.Interfaces;

namespace SchedulerEventDomainTest;
public class DeveloperTest
{
    private readonly Mock<IDeveloperRepository> _developerRepository;
    private readonly Mock<IValidator<DeveloperDto>> _developerValidator;
    private readonly IDeveloperService _developerService;
    private readonly DeveloperDto _developerDto;
    public DeveloperTest()
    {
        _developerRepository = new Mock<IDeveloperRepository>();
        _developerValidator = new Mock<IValidator<DeveloperDto>>(MockBehavior.Strict);
        _developerService = new DeveloperService(_developerRepository.Object, _developerValidator.Object);
        _developerDto = new DeveloperDto
        {
            Name = "name1",
            Email = "ThirdUserUser@Email.Com",
            MobileNumber = "22222222",
            City = "City1",
            BirthDay = new DateTime()
        };
    }

    [Fact]
    public async void Create_Developer_Should_Save()
    {
        //Arrange
        Developer newDeveloper = null;
        _developerRepository.Setup(r => r.CreateDeveloper(It.IsAny<Developer>()))
            .Callback<Developer>(x => newDeveloper = x);

        _developerRepository.Setup(r => r.GetDeveloperByEmail(It.IsAny<string>()))
        .ReturnsAsync(() => null);

        _developerValidator.Setup(validator => validator.ValidateAsync(_developerDto, It.IsAny<CancellationToken>()))
       .ReturnsAsync(new ValidationResult());
        var result = await _developerService.CreateDeveloper(_developerDto);
        _developerRepository.Verify(x => x.CreateDeveloper(It.IsAny<Developer>()), Times.Once);
        _developerRepository.Verify(x => x.GetDeveloperByEmail(It.IsAny<string>()), Times.AtLeastOnce);
        _developerValidator.Verify(x => x.ValidateAsync(_developerDto, It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(_developerDto.Email, newDeveloper.Email);
    }

    [Fact]
    public async void Create_Developer_Already_Exist()
    {
        _developerValidator.Setup(validator => validator.ValidateAsync(_developerDto, It.IsAny<CancellationToken>()))
       .ReturnsAsync(new ValidationResult());

        _developerRepository.Setup(r => r.GetDeveloperByEmail(It.IsAny<string>()))
        .ReturnsAsync(() => new Developer());
        var result = await _developerService.CreateDeveloper(_developerDto);
        _developerValidator.Verify(x => x.ValidateAsync(_developerDto, It.IsAny<CancellationToken>()), Times.Once);
        _developerRepository.Verify(x => x.GetDeveloperByEmail(It.IsAny<string>()), Times.AtLeastOnce);
        Assert.Equal($"The developer with email {_developerDto.Email} already exist.", result.Errors);
    }

    [Fact]
    public async void Create_Developer_Validation_Model_Error()
    {
         //Arrange
        var resultValidation = new ValidationResult();
        resultValidation.Errors.Add(new ValidationFailure("Email", "'Email' is not a valid email address."));
        _developerValidator.Setup(validator => validator.ValidateAsync(_developerDto, It.IsAny<CancellationToken>()))
        .ReturnsAsync(resultValidation);

        var result = await _developerService.CreateDeveloper(_developerDto);
        Assert.True(result.HasError);
        Assert.Equal("'Email' is not a valid email address.", result.Errors);
    }

}