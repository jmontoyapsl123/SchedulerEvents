using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScedulerEventDomain.Services.Interfaces;
using SchedulerEventCommon.Dtos;

namespace SchedulerEventApi.Controllers;
[Route("api/[controller]/[action]")]
public class DeveloperController : Controller
{
    private readonly IDeveloperService _developerService;

    public DeveloperController(IDeveloperService developerService)
    {
        _developerService = developerService;
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<int>> CreateDeveloper([FromBody]DeveloperDto developer)
    {
        var result = await _developerService.CreateDeveloper(developer);
        return Ok(new
        {
            developerId = result
        });
    }

}