using FluentValidation;
using ScedulerEventDomain.Services.Interfaces;
using SchedulerEventCommon.Dtos;
using SchedulerEventRepositories.Entities;
using SchedulerEventRepositories.Repositories.Interfaces;

namespace ScedulerEventDomain.Services.Implementations;
public class DeveloperService : IDeveloperService
{
    private static IDeveloperRepository _developerRepository;
    private readonly IValidator<DeveloperDto> _developerValidator;
    public DeveloperService(IDeveloperRepository developerRepository, IValidator<DeveloperDto> developerValidator)
    {
        _developerRepository = developerRepository;
        _developerValidator = developerValidator;
    }

    public async Task<ResponseDto<int>> CreateDeveloper(DeveloperDto developerDto)
    {
        var validation = await _developerValidator.ValidateAsync(developerDto);
        ResponseDto<int> response = new ResponseDto<int>();
        response.HasError = true;
        if (!validation.IsValid)
        {
            response.Errors = string.Join(", ", validation.Errors);
            return response;
        }

        var developer = await _developerRepository.GetDeveloperByEmail(developerDto.Email);
        if(developer != null)
        {
            response.Errors = $"The developer with email {developerDto.Email} already exist.";
            return response;
        }

        Developer newDeveloper = new Developer
        {
            BirthDay = developerDto.BirthDay,
            City = developerDto.City,
            Email = developerDto.Email,
            MobileNumber = developerDto.MobileNumber,
            Name = developerDto.Name
        };

        await _developerRepository.CreateDeveloper(newDeveloper);
        response.HasError = false;
        response.Result = newDeveloper.Id;
        return response;
    }
}