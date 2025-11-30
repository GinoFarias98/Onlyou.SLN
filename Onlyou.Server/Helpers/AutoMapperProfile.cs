using AutoMapper;
using Onlyou.BD.Data.Entidades;
using Onlyou.Shared.DTOS;
using Onlyou.Shared.DTOS.Caja;
using Onlyou.Shared.DTOS.Categorias;
using Onlyou.Shared.DTOS.Color;
using Onlyou.Shared.DTOS.Marca;
using Onlyou.Shared.DTOS.Movimiento;
using Onlyou.Shared.DTOS.ObservacionCaja;
using Onlyou.Shared.DTOS.ObservacionPago;
using Onlyou.Shared.DTOS.Pago;
using Onlyou.Shared.DTOS.Pedidos;
using Onlyou.Shared.DTOS.Pedidos.EstadoPedido;
using Onlyou.Shared.DTOS.Pedidos.PedidoItem;
using Onlyou.Shared.DTOS.Producto;
using Onlyou.Shared.DTOS.Proveedor;
using Onlyou.Shared.DTOS.Talle;
using Onlyou.Shared.DTOS.TipoMovimento;
using Onlyou.Shared.DTOS.TipoPago;
using Onlyou.Shared.DTOS.TipoProducto;
using Onlyou.Shared.Enums;


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


            //// Mapeos para Pedidos
            CreateMap<Pedido, GetPedidosDTO>()
                .ForMember(dest => dest.EstadoPedidoNombre, opt => opt.MapFrom(src => src.EstadoPedido.Nombre))
                .ForMember(dest => dest.PedidoItems, opt => opt.MapFrom(src => src.PedidoItems));


            CreateMap<PostPedidoDTO, Pedido>()
                .ForMember(dest => dest.EstadoEntrega, opt => opt.Ignore()) // Se setea manualmente
                .ForMember(dest => dest.EstadoPago, opt => opt.Ignore())    // Se setea manualmente
                .ForMember(dest => dest.FechaGenerado, opt => opt.Ignore()) // Se setea manualmente
                .ForMember(dest => dest.FechaPedidoAProveedor, opt => opt.Ignore()) // Se setea manualmente
                .ForMember(dest => dest.MontoEntregado, opt => opt.Ignore()) // Se setea manualmente
                .ForMember(dest => dest.MontoPagado, opt => opt.Ignore());  // Se setea manualmente

            //esto no se
            CreateMap<PutPedidoDTO, Producto>()
                .ForMember(dest => dest.FecUltimaModificacion, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Mapeos para PedidoItems
            CreateMap<PedidoItem, GetPedidoItemDTO>()
                .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.Producto.Nombre))
                .ForMember(dest => dest.ProductoImagen, opt => opt.MapFrom(src => src.Producto.Imagen));

            CreateMap<PostPedidoItemDTO, PedidoItem>();
            CreateMap<PutPedidoItemDTO, PedidoItem>();

            // Mapeo para EstadoPedido
            CreateMap<EstadoPedido, EstadoPedidoDTO>();
            CreateMap<EstadoPedidoDTO, EstadoPedido>();

            //CreateMap<PutProductoDTO, Producto>()
            //    .ForMember(dest => dest.Imagen, opt => opt.Ignore())
            //    .ForMember(dest => dest.ProductosColores, opt => opt.MapFrom(src =>
            //        src.Colores.Select(id => new ProductoColor { ColorId = id })))
            //    .ForMember(dest => dest.ProductosTalles, opt => opt.MapFrom(src =>
            //        src.Talles.Select(id => new ProductoTalle { TalleId = id })));


            // Caja


            CreateMap<Caja, GetCajaDTO>().ReverseMap();
            CreateMap<PostCajaDTO, Caja>();
            CreateMap<PutEstadoCajaDTO, Caja>();

            // ObservacionCaja


            CreateMap<ObservacionCaja, GetObservacionCajaDTO>();
            CreateMap<PostObservacionCajaDTO, ObservacionCaja>(); // opcional
            CreateMap<Caja, GetCajaDTO>()
                .ForMember(d => d.Observaciones, o => o.MapFrom(s => s.Observaciones));


            // Tipo Movimiento


            CreateMap<TipoMovimiento, GetTipoMovimeintoDTO>().ReverseMap();
            CreateMap<PostTipoMovimientoDTO, TipoMovimiento>();
            CreateMap<PutTipoMovimiento, TipoMovimiento>();


            CreateMap<Movimiento, GetMovimientoDTO>()
              .ForMember(dest => dest.EstadoMovimiento,
                         opt => opt.MapFrom(src => src.EstadoMovimiento))

              // Caja
              .ForMember(dest => dest.CajaId,
                         opt => opt.MapFrom(src => src.CajaId))

              // TipoMovimiento
              .ForMember(dest => dest.TipoMovimientoNombre,
                         opt => opt.MapFrom(src =>
                              src.TipoMovimiento != null
                              ? src.TipoMovimiento.Nombre
                              : null))
              .ForMember(dest => dest.TipoMovimientoSigno,
                         opt => opt.MapFrom(src =>
                              src.TipoMovimiento != null
                              ? (SignoTipoMovimientoDto)src.TipoMovimiento.signo
                              : SignoTipoMovimientoDto.Suma))   // valor por defecto

              // Proveedor
              .ForMember(dest => dest.ProveedorNombre,
                         opt => opt.MapFrom(src =>
                              src.Proveedor != null
                              ? src.Proveedor.Nombre
                              : null))
              .ForMember(dest => dest.RazonSocial,
                         opt => opt.MapFrom(src =>
                              src.Proveedor != null
                              ? src.Proveedor.RazonSocial
                              : null))

              // Pedido
              .ForMember(dest => dest.PedidoDescripcion,
                         opt => opt.MapFrom(src =>
                              src.Pedido != null
                              ? src.Pedido.Descripcion
                              : null));


            CreateMap<PostMovimientoDTO, Movimiento>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // se genera solo
                .ForMember(dest => dest.Caja, opt => opt.Ignore())
                .ForMember(dest => dest.TipoMovimiento, opt => opt.Ignore())
                .ForMember(dest => dest.Proveedor, opt => opt.Ignore())
                .ForMember(dest => dest.Pedido, opt => opt.Ignore());

            CreateMap<PutMovimientoDTO, Movimiento>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Pagos, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoMovimiento, opt => opt.MapFrom(src => src.EstadoMovimiento));


            // PUT Estado → DTO ▼ Movimiento (solo estado y nota/desc)
            CreateMap<PutEstadoMovimientoDTO, Movimiento>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoMovimiento, opt => opt.MapFrom(src => src.EstadoMovimiento))
                .ForMember(dest => dest.Descripcion,
                           opt => opt.MapFrom((src, dest) =>
                               $"{(dest.Descripcion ?? "").Trim()} | Nota: {src.Descripcion.Trim()}"
                           ));


            // Tipo Pago


            CreateMap<TipoPago, TipoPagoDTO>().ReverseMap();


            // Pago


            // -------- GET --------
            CreateMap<Pago, GetPagoDTO>()
                .ForMember(d => d.Situacion, o => o.MapFrom(s => (SituacionPagoDto)s.Situacion))
                .ForMember(d => d.TipoPagoNombre, o => o.MapFrom(s => s.TipoPago.Nombre))
                .ForMember(d => d.MovimientoDescripcion, o => o.MapFrom(s => s.Movimiento.Descripcion))
                .ForMember(d => d.Observaciones, o => o.MapFrom(s => s.Observaciones));

            CreateMap<PostPagoDTO, Pago>()
                .ForMember(d => d.Situacion, o => o.MapFrom(s => (Situacion)s.Situacion));

            CreateMap<PutPagoDTO, Pago>()
                .ForMember(d => d.Situacion, o => o.MapFrom(s => (Situacion)s.Situacion));

            // Observacion Pago


            CreateMap<ObservacionPago, GetObservacionPagoDTO>();
            CreateMap<GetObservacionPagoDTO, ObservacionPago>(); // opcional
            CreateMap<Pago, GetPagoDTO>()
                .ForMember(d => d.Observaciones, o => o.MapFrom(s => s.Observaciones));
            CreateMap<PostObservacionPagoDTO, ObservacionPago>();

        }
    }
}
