using AutoMapper;
using Onlyou.BD.Data.Entidades;
using Onlyou.Shared.DTOS.Categorias;
using Onlyou.Shared.DTOS.Color;

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

            CreateMap<Color, GetColorDTO>();
            CreateMap<PostColorDTO, Color>();
            CreateMap<PutColorDTO, Color>();

        }
    }
}
