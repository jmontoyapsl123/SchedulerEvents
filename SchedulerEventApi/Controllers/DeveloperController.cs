using Microsoft.AspNetCore.Mvc;
using ScedulerEventDomain.Services;
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
    public async Task<ActionResult<int>> CreateDeveloper([FromBody]DeveloperDto developer)
    {
        var result = await _developerService.CreateDeveloper(developer);
        return Ok(new
        {
            items = result
        });
    }

}