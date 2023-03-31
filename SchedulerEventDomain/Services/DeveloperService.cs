using SchedulerEventCommon.Dtos;
using SchedulerEventRepositories.Entities;
using SchedulerEventRepositories.Repositories;

namespace ScedulerEventDomain.Services;
public class DeveloperService : IDeveloperService
{
    private static IDeveloperRepository _developerRepository;

    public DeveloperService(IDeveloperRepository developerRepository)
    {
        _developerRepository = developerRepository;
    }

    public async Task<int> CreateDeveloper(DeveloperDto developerDto)
    {
        Developer developer = new Developer
        {
            BirthDay = developerDto.BirthDay,
            City = developerDto.City,
            Email = developerDto.Email,
            MobileNumber = developerDto.MobileNumber,
            Name = developerDto.Name
        };

        await _developerRepository.CreateDeveloper(developer);
        return developer.Id;
    }
}