using Application.Soundtracks.Commands.Upload;
using Application.Soundtracks.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
public class SoundtrackController(IMediator mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAll(GetAllQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Upload(UploadFileCommand uploadFileCommand)
    {
        var result = await mediator.Send(uploadFileCommand);
        return Ok(result);
    }
}