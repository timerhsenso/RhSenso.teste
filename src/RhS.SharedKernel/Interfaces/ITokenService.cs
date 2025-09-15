using RhS.SharedKernel.DTOs;
using System.Security.Claims;

namespace RhS.SharedKernel.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string userName, List<PermissionDto> permissions);
        bool ValidateToken(string token);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
