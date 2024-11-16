using Application.Streaming.Queries.Stream;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
public class StreamingController(IMediator mediator) : Controller
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Index(Guid id)
    {
        var result = await mediator.Send(new StreamCommand(id));

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return result.Body;
    }
}