using Domain.Entities;

namespace Application.Abstractions;

public interface ITokenGeneratorService
{
    Task<string> GenerateJwtToken(ApplicationUser user, List<string> userRoles);
}