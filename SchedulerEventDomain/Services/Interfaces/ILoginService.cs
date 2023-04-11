using SchedulerEventCommon.Dtos;

namespace ScedulerEventDomain.Services.Interfaces;
public interface ILoginService
{
    Task<ResponseDto<string>> Login(LoginDto login);
    Task<ResponseDto<int>> CreateUser(LoginDto login);
}