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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [AllowAnonymous]
    public async Task<ActionResult<int>> CreateEventInvitation([FromBody] EventInvitationDto eventInvitationDto)
    {
        var result = await _eventInvitationService.CreateEventInvitation(eventInvitationDto);
        return Ok(new
        {
            developerId = result
        });
    }

    [HttpGet]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [AllowAnonymous]
    public async Task<ActionResult<int>> GetEventInvitationReport(ReportParamDto reportParamDto)
    {
        var result = await _eventInvitationService.GetEventInvitationReport(reportParamDto);
        return Ok(new
        {
            result = result
        });
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> AcceptEventInvitation([FromQuery] string hash)
    {
        var result = await _eventInvitationService.AcceptEventInvitation(hash);
        return Ok(new
        {
            result = result
        });
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> RejectEventInvitation([FromQuery] string hash)
    {
        var result = await _eventInvitationService.RejectEventInvitation(hash);
        return Ok(new
        {
            result = result
        });
    }

}