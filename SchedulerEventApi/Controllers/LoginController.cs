using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScedulerEventDomain.Services.Interfaces;
using SchedulerEventCommon.Dtos;

namespace SchedulerEventApi.Controllers;
[Route("api/[controller]/[action]")]
public class LoginController : Controller
{
    private readonly ILoginService _loginServiceService;

    public LoginController(ILoginService loginServiceService)
    {
        _loginServiceService = loginServiceService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<int>> Login([FromBody]LoginDto login)
    {
        var result = await _loginServiceService.Login(login);
        return Ok(new
        {
            result
        });
    }

     [HttpPost]
     [AllowAnonymous]
    public async Task<ActionResult<int>> CreateUser([FromBody]LoginDto login)
    {
        var result = await _loginServiceService.CreateUser(login);
        return Ok(new
        {
            result
        });
    }

}