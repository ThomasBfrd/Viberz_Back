using MediatR;
using Microsoft.AspNetCore.Mvc;
using Viberz.Application.DTO.Auth;
using Viberz.Application.DTO.Xp;
using Viberz.Application.Models;
using Viberz.Application.Utilities;

namespace Viberz.Viberz.API.Controllers;

[Route("api/xp-history")]
[ApiController]
public class XpHistoryController : ControllerBase
{
    private readonly JwtDecode _jwtDecode;
    private readonly IMediator _mediator;

    public XpHistoryController(JwtDecode jwtDecode, IMediator mediator)
    {
        _jwtDecode = jwtDecode;
        _mediator = mediator;
    }

    [HttpPost("add-history-game")]
    public async Task<IActionResult> AddHistoryGame([FromBody] AddXpHistoryGame addXpHistoryGame)
    {
        UserJwtConnexion? token = _jwtDecode.GetUserAuthInformations(Request.Headers.Authorization.ToString()) ??
            throw new Exception("UserId is missing in the JWT.");

        UserXpDTO result = await _mediator.Send(new AddXpHistory(token.UserId, addXpHistoryGame));

        return Ok(result);
    }
}
