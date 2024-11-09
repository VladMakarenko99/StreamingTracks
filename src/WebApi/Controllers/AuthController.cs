using Application.Abstractions;
using Application.DTOs.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IUserAuthenticationService _userAuthenticationService;

    public AuthController(IUserAuthenticationService userAuthenticationService) =>
        _userAuthenticationService = userAuthenticationService;

    [HttpPost("sign-in")]
    public async Task<IActionResult> Login([FromBody] LoginUserDTO
        loginUserDto)
    {
        var result = await _userAuthenticationService.LoginAsync(loginUserDto);
        if (string.IsNullOrEmpty(result)) return Unauthorized("Login failed.");
        
        return Ok(result);
    }
    
    [HttpGet]
    [Authorize]
    public  IActionResult GetAuth()
    {
        return Ok("Authorized");
    }
}