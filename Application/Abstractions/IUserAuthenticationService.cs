using Application.DTOs.Login;

namespace Application.Abstractions;

public interface IUserAuthenticationService
{
    Task<string> LoginAsync(LoginUserDTO loginUserDto);
}