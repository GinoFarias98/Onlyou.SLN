using AutoMapper;
using Onlyou.BD.Data.Entidades;
using Onlyou.Shared.DTOS.Categorias;
using Onlyou.Shared.DTOS.Color;
using Onlyou.Shared.DTOS.Talle;
using Onlyou.Shared.DTOS.Marca;
using Onlyou.Shared.DTOS.Proveedor;
using Onlyou.Shared.DTOS.TipoProducto;

namespace Onlyou.Server.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Categoria ===================================================

            CreateMap<Categoria, CrearCategoriasDTO>().ReverseMap();
            CreateMap<Categoria, GetCategoriasDTO>().ReverseMap();
            CreateMap<Categoria, EditarCategoriasDTO>().ReverseMap();

            // Color ===================================================

            CreateMap<Color, GetColorDTO>().ReverseMap();
            CreateMap<PostColorDTO, Color>();
            CreateMap<PutColorDTO, Color>();

            // Tipo Productos

            CreateMap<TipoProducto, GetTipoProductoDTO>().ReverseMap();
            CreateMap<PostTipoProductoDTO, TipoProducto>();
            CreateMap<PutTipoProductoDTO, TipoProducto>();

            //Talle
            CreateMap<Talle, TallesDTO>().ReverseMap();
            CreateMap<TallesDTO, Talle>();

        }
    }
}
