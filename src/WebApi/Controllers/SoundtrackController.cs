using Application.Soundtracks.Commands.Delete;
using Application.Soundtracks.Commands.Upload;
using Application.Soundtracks.Queries.GetAll;
using Application.Soundtracks.Queries.GetById;
using Application.Soundtracks.Queries.GetImage;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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

    [HttpGet("image/{fileName}")]
    public async Task<IActionResult> Image(string fileName)
    {
        var result = await mediator.Send(new GetImageQuery(fileName));
        
        if (!result.IsSuccess || result.Body is null)
        {
            return BadRequest(result.Error);
        }
        
        const string contentType = "image/jpeg";
        return File(result.Body, contentType);
    }
}