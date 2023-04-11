using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ScedulerEventDomain.Services.Interfaces;
using SchedulerEventCommon.Dtos;
using SchedulerEventCommon.Utilities;
using SchedulerEventRepositories.Entities;
using SchedulerEventRepositories.Repositories.Interfaces;

namespace ScedulerEventDomain.Services.Implementations;
public class LoginService : ILoginService
{
    private readonly IConfiguration _configuration;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordUtility _passwordUtility;
    public LoginService(IValidator<LoginDto> loginValidator, IUserRepository userRepository, IConfiguration configuration, IPasswordUtility passwordUtility)
    {
        _loginValidator = loginValidator;
        _userRepository = userRepository;
        _configuration = configuration;
        _passwordUtility = passwordUtility;
    }

    public async Task<ResponseDto<string>> Login(LoginDto login)
    {
        var validation = await _loginValidator.ValidateAsync(login);
        ResponseDto<string> response = new ResponseDto<string>();
        response.HasError = true;
        if (!validation.IsValid)
        {
            response.Errors = string.Join(", ", validation.Errors);
            return response;
        }

        var user = await _userRepository.GetUserByEmail(login.Email);
        if (user == null || !_passwordUtility.VerifyPassword(login.Password, user.Password, user.PasswordSalt))
        {
            response.Errors = "User or password invalid";
            return response;
        }

        response.HasError = false;
        response.Result = GenerateToken();
        return response;
    }

    public async Task<ResponseDto<int>> CreateUser(LoginDto login)
    {
        var validation = await _loginValidator.ValidateAsync(login);
        ResponseDto<int> response = new ResponseDto<int>();
        response.HasError = true;
        if (!validation.IsValid)
        {
            response.Errors = string.Join(", ", validation.Errors);
            return response;
        }

        var validateUser = await _userRepository.GetUserByEmail(login.Email);
        if (validateUser != null)
        {
            response.Errors = "The user already exist.";
            return response;
        }

        var passwordHashed = _passwordUtility.HashPasword(login.Password, out var salt);
        User user = new User
        {
            Email = login.Email,
            Password = passwordHashed,
            PasswordSalt = salt
        };

        await _userRepository.CreateUser(user);
        response.HasError = false;
        response.Result = user.Id;
        return response;
    }

    private string GenerateToken()
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AuthenticationToken:Token").Value));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            claims: new List<Claim>(),
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: signinCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

        return tokenString;
    }

}