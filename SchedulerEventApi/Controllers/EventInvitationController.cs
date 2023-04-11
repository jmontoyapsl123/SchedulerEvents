using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScedulerEventDomain.Services.Interfaces;
using SchedulerEventCommon.Dtos;

namespace SchedulerEventApi.Controllers;
[Route("api/[controller]/[action]")]
public class EventInvitationController : Controller
{
    private readonly IEventInvitationService _eventInvitationService;

    public EventInvitationController(IEventInvitationService eventInvitationService)
    {
        _eventInvitationService = eventInvitationService;
    }

    [HttpPost]
    [AllowAnonymous]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<int>> CreateEventInvitation([FromBody]EventInvitationDto eventInvitationDto)
    {
        var result = await _eventInvitationService.CreateEventInvitation(eventInvitationDto);
        return Ok(new
        {
            developerId = result
        });
    }

}