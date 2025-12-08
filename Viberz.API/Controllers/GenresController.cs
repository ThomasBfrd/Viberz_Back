using MediatR;
using Microsoft.AspNetCore.Mvc;
using Viberz.Application.Queries.Genres;

namespace Viberz.API.Controllers;

[Route("api/genres")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IMediator _mediator;

    public GenresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<string>>> GetGenres()
    {
        List<string> result = await _mediator.Send(new GetGenresQuery());
        return Ok(result);
    }
}
