using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScedulerEventDomain.Services.Interfaces;
using SchedulerEventCommon.Dtos;

namespace SchedulerEventApi.Controllers;
[Route("api/[controller]/[action]")]
public class EventController : Controller
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpPost]
    [AllowAnonymous]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<int>> CreateEvent([FromBody]EventDto eventDto)
    {
        var result = await _eventService.CreateEvent(eventDto);
        return Ok(new
        {
            developerId = result
        });
    }

}