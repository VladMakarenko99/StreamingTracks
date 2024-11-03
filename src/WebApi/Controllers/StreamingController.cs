using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
public class StreamingController : Controller
{
    [HttpGet("{fileName}")]
    public IActionResult Index(string fileName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "FileTest", fileName.Replace("%20", " "));
        var fs = new FileStream(path, FileMode.Open);

        return new FileStreamResult(fs, "audio/mp3")
        {
            EnableRangeProcessing = true,
        };
    }
}