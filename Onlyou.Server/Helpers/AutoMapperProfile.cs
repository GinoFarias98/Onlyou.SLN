using AutoMapper;
using Onlyou.BD.Data.Entidades;
using Onlyou.Shared.DTOS.Caja;
using Onlyou.Shared.DTOS.Categorias;
using Onlyou.Shared.DTOS.Color;
using Onlyou.Shared.DTOS.Marca;
using Onlyou.Shared.DTOS.ObservacionCaja;
using Onlyou.Shared.DTOS.Producto;
using Onlyou.Shared.DTOS.Proveedor;
using Onlyou.Shared.DTOS.Talle;
using Onlyou.Shared.DTOS.TipoMovimento;
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
            CreateMap<EditarCategoriasDTO, Categoria>()
                .ForMember(dest => dest.Imagen, opt => opt.Ignore());

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

            //Proveedor
            CreateMap<Proveedor, GetProveedorDTO>().ReverseMap();
            CreateMap<PostProveedorDTO, Proveedor>();
            CreateMap<PutProveedorDTO, Proveedor>();

            //Marca
            CreateMap<Marca, GetMarcaDTO>().ReverseMap();
            CreateMap<PostMarcaDTO, Marca>();
            CreateMap<PutMarcaDTO, Marca>();


            //Producto

            CreateMap<Producto, GetProductoDTO>()
                .ForMember(dest => dest.ProveedorNombre, opt => opt.MapFrom(src => src.Proveedor.Nombre))
                .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria.Nombre))
                .ForMember(dest => dest.TipoProductoNombre, opt => opt.MapFrom(src => src.TipoProducto.Nombre))
                .ForMember(dest => dest.MarcaNombre, opt => opt.MapFrom(src => src.Marca.Nombre))

                // Mapear IDs simples
                .ForMember(dest => dest.Colores, opt => opt.MapFrom(src => src.ProductosColores.Select(pc => pc.ColorId)))
                .ForMember(dest => dest.Talles, opt => opt.MapFrom(src => src.ProductosTalles.Select(pt => pt.TalleId)))

                // Mapear detalles completos
                .ForMember(dest => dest.ColoresDetalle, opt => opt.MapFrom(src => src.ProductosColores.Select(pc => pc.Color)))
                .ForMember(dest => dest.TallesDetalle, opt => opt.MapFrom(src => src.ProductosTalles.Select(pt => pt.Talle)));



            CreateMap<PostProductoDTO, Producto>()
                .ForMember(dest => dest.ProductosColores, opt => opt.MapFrom(src =>
                    src.Colores.Select(id => new ProductoColor { ColorId = id })))
                .ForMember(dest => dest.ProductosTalles, opt => opt.MapFrom(src =>
                    src.Talles.Select(id => new ProductoTalle { TalleId = id })));

            CreateMap<PutProductoDTO, Producto>()
                .ForMember(dest => dest.Imagen, opt => opt.Ignore())
                .ForMember(dest => dest.ProductosColores, opt => opt.Ignore())
                .ForMember(dest => dest.ProductosTalles, opt => opt.Ignore());

            //CreateMap<PutProductoDTO, Producto>()
            //    .ForMember(dest => dest.Imagen, opt => opt.Ignore())
            //    .ForMember(dest => dest.ProductosColores, opt => opt.MapFrom(src =>
            //        src.Colores.Select(id => new ProductoColor { ColorId = id })))
            //    .ForMember(dest => dest.ProductosTalles, opt => opt.MapFrom(src =>
            //        src.Talles.Select(id => new ProductoTalle { TalleId = id })));


            // Caja


            CreateMap<Caja, GetCajaDTO>().ReverseMap();
            CreateMap<PostCajaDTO, Caja>();
            CreateMap<PutCajaDTO, Caja>();


            // ObservacionCaja


            CreateMap<ObservacionCaja, GetObservacionCajaDTO>();
            CreateMap<PostObservacionCajaDTO, ObservacionCaja>(); // opcional
            CreateMap<Caja, GetCajaDTO>()
                .ForMember(d => d.Observaciones, o => o.MapFrom(s => s.Observaciones));


            // Tipo Movimiento


            CreateMap<TipoMovimiento, GetTipoMovimeintoDTO>().ReverseMap();
            CreateMap<PostTipoMovimientoDTO, TipoMovimiento>();
            CreateMap<PutTipoMovimiento, TipoMovimiento>();





        }
    }
}
