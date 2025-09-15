
using AutoMapper;
using RhS.Api.DTOs.SEG;
using RhS.SEG.Core.Entities;

namespace RhS.Api.Mapping.SEG;

public sealed class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<Usuario, UsuarioListDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(u => u.CdUsuario))
            .ForCtorParam("CdUsuario", opt => opt.MapFrom(u => u.CdUsuario))
            .ForCtorParam("DcUsuario", opt => opt.MapFrom(u => u.DcUsuario))
            .ForCtorParam("EmailUsuario", opt => opt.MapFrom(u => u.EmailUsuario))
            .ForCtorParam("FlAtivo", opt => opt.MapFrom(u => u.FlAtivo))
            .ForCtorParam("NoUser", opt => opt.MapFrom(u => u.NoUser));

        CreateMap<Usuario, UsuarioFormDto>().ReverseMap();
    }
}
