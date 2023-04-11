using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Moq;
using ScedulerEventDomain.Services.Implementations;
using ScedulerEventDomain.Services.Interfaces;
using SchedulerEventCommon.Dtos;
using SchedulerEventCommon.Utilities;
using SchedulerEventRepositories.Entities;
using SchedulerEventRepositories.Repositories.Interfaces;

namespace SchedulerEventDomainTest;

public class UserTest
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly Mock<IConfiguration> _configuration;
    private readonly Mock<IValidator<LoginDto>> _loginValidator;
    private readonly Mock<IPasswordUtility> _passwordUtility;
    private readonly ILoginService _loginService;
    private readonly LoginDto _loginDto;
    public UserTest()
    {
        _mockRepository = new Mock<IUserRepository>();
        _loginValidator = new Mock<IValidator<LoginDto>>(MockBehavior.Strict);
        _configuration = new Mock<IConfiguration>();
        _passwordUtility = new Mock<IPasswordUtility>();
        _loginService = new LoginService(_loginValidator.Object, _mockRepository.Object, _configuration.Object, _passwordUtility.Object);
        _loginDto = new LoginDto
        {
            Email = "ThirdUserUser@Email.Com",
            Password = "password123"
        };
    }


    [Fact]
    public async void Create_User_Should_Save()
    {
        //Arrange
        User newUser = null;
        _mockRepository.Setup(r => r.CreateUser(It.IsAny<User>()))
            .Callback<User>(x => newUser = x);

        _mockRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>()))
               .ReturnsAsync(() => null);

        _loginValidator.Setup(validator => validator.ValidateAsync(_loginDto, It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ValidationResult());

        var result = await _loginService.CreateUser(_loginDto);
        _mockRepository.Verify(x => x.CreateUser(It.IsAny<User>()), Times.Once);
        _mockRepository.Verify(x => x.GetUserByEmail(It.IsAny<string>()), Times.AtLeastOnce);
        Assert.Equal(_loginDto.Email, newUser.Email);

    }

    [Fact]
    public async void Create_User_Should_Not_Save_Error_Email()
    {
        //Arrange
        var resultValidation = new ValidationResult();
        resultValidation.Errors.Add(new ValidationFailure("Email", "'Email' is not a valid email address."));
        _loginValidator.Setup(validator => validator.ValidateAsync(_loginDto, It.IsAny<CancellationToken>()))
        .ReturnsAsync(resultValidation);

        var result = await _loginService.CreateUser(_loginDto);
        Assert.True(result.HasError);
        Assert.Equal("'Email' is not a valid email address.", result.Errors);
    }

    [Fact]
    public async void Create_User_Should_User_Exist()
    {
        //Arrange
        _mockRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(new User());

        _loginValidator.Setup(validator => validator.ValidateAsync(_loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var result = await _loginService.CreateUser(_loginDto);
        Assert.True(result.HasError);
        Assert.Equal("The user already exist.", result.Errors);
    }

    [Fact]
    public async void Login_Should_Create_Token()
    {
        var user = new User
        {
            Email = "ThirdUserUser@Email.Com",
            Password = "BB3E100C36C4C32C99817AFF393C0D2132B2BA4061CBEA73970EEFDEFDAD58BDD31BEC9D2926CC5D0E7ED3AA7DAD7550EFE3E4F56B40A411400597F7C6A43700",
            Id = 0,
            PasswordSalt = new byte[64]
        };

        _loginValidator.Setup(validator => validator.ValidateAsync(_loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(user);

        _passwordUtility.Setup(repo => repo.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<byte[]>()))
            .Returns(true);

        _configuration.Setup(repo => repo.GetSection(It.IsAny<string>()).Value).Returns("!SomeSercret2023*!");

        var result = await _loginService.Login(_loginDto);

        _mockRepository.Verify(x => x.GetUserByEmail(It.IsAny<string>()), Times.Once);
        _passwordUtility.Verify(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<byte[]>()), Times.Once);
        _configuration.Verify(x => x.GetSection(It.IsAny<string>()).Value, Times.Once);
        Assert.False(result.HasError);
        Assert.NotEmpty(result.Result);
    }

    [Fact]
    public async void Login_Should_Not_Create_Token_Uses_Does_Not_Exist()
    {
        _loginValidator.Setup(validator => validator.ValidateAsync(_loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync((User)null);

        var result = await _loginService.Login(_loginDto);
        _loginValidator.Verify(x => x.ValidateAsync(_loginDto, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(x => x.GetUserByEmail(It.IsAny<string>()), Times.Once);
        Assert.True(result.HasError);
        Assert.Null(result.Result);
        Assert.Equal("User or password invalid", result.Errors);
    }

    [Fact]
    public async void Login_Should_Not_Create_Token_Invalid_Password()
    {
        _loginValidator.Setup(validator => validator.ValidateAsync(_loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(new User());
        _passwordUtility.Setup(repo => repo.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<byte[]>()))
            .Returns(false);
        var result = await _loginService.Login(_loginDto);
        _loginValidator.Verify(x => x.ValidateAsync(_loginDto, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(x => x.GetUserByEmail(It.IsAny<string>()), Times.Once);
        _passwordUtility.Verify(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<byte[]>()), Times.Once);

        Assert.True(result.HasError);
        Assert.Null(result.Result);
        Assert.Equal("User or password invalid", result.Errors);
    }
}