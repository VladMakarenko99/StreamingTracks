using Application.Abstractions;
using Application.DTOs.Login;
using Domain.Entities;

namespace Application.Implementations;

public class UserAuthenticationService(ITokenGeneratorService generatorService) : IUserAuthenticationService
{
    public async Task<string> LoginAsync(LoginUserDTO loginUserDto)
    {
        if (loginUserDto is { Email: "admin", Password: "admin1234" })
        {
            var user = new ApplicationUser()
            {
                FirstName = "Vlad",
                LastName = "Kovtun"
            };
            var token = await generatorService.GenerateJwtToken(user ?? new ApplicationUser(),
                ["Admin"]);

            return token;
        }

        return string.Empty;
    }
}