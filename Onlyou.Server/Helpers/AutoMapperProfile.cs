using AutoMapper;
using Onlyou.BD.Data.Entidades;
using Onlyou.Shared.DTOS.Categorias;

namespace Onlyou.Server.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Categoria, CrearCategoriasDTO>().ReverseMap();
            CreateMap<Categoria, GetCategoriasDTO>().ReverseMap();
            CreateMap<Categoria, EditarCategoriasDTO>().ReverseMap();


        }
    }
}
