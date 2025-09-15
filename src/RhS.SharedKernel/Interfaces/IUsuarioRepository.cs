using RhS.SharedKernel.DTOs;

namespace RhS.SharedKernel.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<UserDto?> GetByUsernameAsync(string cdUsuario, CancellationToken ct = default);
        Task<List<PermissionDto>> GetPermissionsAsync(string cdUsuario, CancellationToken ct = default);
    }
}
