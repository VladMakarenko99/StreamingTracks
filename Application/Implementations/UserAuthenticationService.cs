using Application.Abstractions;
using Application.DTOs.Login;

namespace Application.Implementations;

public class UserAuthenticationService : IUserAuthenticationService
{
    public async Task<string> LoginAsync(LoginUserDTO loginUserDto)
    {
        if (loginUserDto is { Email: "admin", Password: "admin1234" })
        {
            return "JWT TOKEN";
        }

        return string.Empty;
    }
}