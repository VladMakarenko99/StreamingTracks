using Application.Soundtracks.Commands.Delete;
using Application.Soundtracks.Commands.IncreaseListeningCount;
using Application.Soundtracks.Commands.Upload;
using Application.Soundtracks.Queries.GetAll;
using Application.Soundtracks.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SoundtrackController(IMediator mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllQuery();
        
        var result = await mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var result = await mediator.Send(new GetBySlugQuery(slug));
        
        return Ok(result);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var result = await mediator.Send(new UploadFileCommand(file));

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteFileCommand(id));

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> IncreaseListeningCount(Guid id)
    {
        var result = await mediator.Send(new IncreaseListeningCountCommand(id));

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}