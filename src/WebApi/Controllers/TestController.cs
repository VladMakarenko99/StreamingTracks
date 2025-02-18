using Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(IAwsS3Service awsS3Service) : Controller
{
    // [HttpGet]
    // public async Task<IActionResult> Get()
    // {
    //     var buckets = await awsS3Service.GetAsync();
    //
    //     return Ok(buckets.Buckets);
    // }

}