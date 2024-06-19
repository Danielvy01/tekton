using AutoMapper;
using Tekton.Configuration.Application.Features.Producto.Command.Create;
using Tekton.Configuration.Application.Features.Producto.Command.Update;
using Tekton.Configuration.Application.Features.Producto.VIewModel.Response;
using Tekton.Configuration.Application.Wrappers;
using Tekton.Configuration.Damain.Entities.Producto;
using Tekton.RedisCaching;

namespace Tekton.Configuration.Application.Mappings;

/// <summary>
/// AutomapperProfile
/// </summary>
[ExcludeFromCodeCoverage]
public class AutomapperProfile : Profile
{
    public AutomapperProfile(IRedisCacheService _redisCacheService)
    {
        CreateMap<ProductoEntity, GetListProductoResponse>()
            .ForPath(s => s.StatusName,
            opt => opt.MapFrom(src => src.ObtenerElStatus(_redisCacheService))
            ).ForPath(s => s.FinalPrice,
            opt => opt.MapFrom(src => src.ObtenerFinalPrice())
            );
        CreateMap<InsertProductoCommand, ProductoEntity>();
        CreateMap<UpdateProductoCommand, ProductoEntity>();


    }
}
