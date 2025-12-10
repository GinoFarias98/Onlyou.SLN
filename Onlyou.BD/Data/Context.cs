using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data
{
    public class Context : IdentityDbContext
    {
        public DbSet<PedidoItem> PedidoItems { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Color> Colores { get; set; }
        public DbSet<EstadoPedido> EstadoPedidos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<ProductoColor> ProductoColores { get; set; }
        public DbSet<ProductoTalle> ProductoTalles { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Talle> Talles { get; set; }
        public DbSet<TipoProducto> TipoProductos { get; set; }
        public DbSet<TipoMovimiento> TipoMovimientos { get; set; }
        public DbSet<Caja> Cajas { get; set; }
        public DbSet<ObservacionCaja> ObservacionCajas { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<ObservacionPago> ObservacionPagos { get; set; }
        public DbSet<ObservacionPedido> ObservacionPedidos { get; set; }


        public DbSet<TipoPago> TipoPagos { get; set; }


        // Es necesario trabajar con DNI en Clientes, para indexar mejor, AGREGAR

        public Context(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }


            modelBuilder.Entity<ProductoColor>().HasKey(x => new { x.ProductoId, x.ColorId });
            modelBuilder.Entity<ProductoTalle>().HasKey(x => new { x.ProductoId, x.TalleId });


            base.OnModelCreating(modelBuilder);


        }

    }

}
