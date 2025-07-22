using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data
{
    public class Context : DbContext
    {
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<CarritoItem> CarritoItems { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Color> Colores { get; set; }
        public DbSet<EstadoPedido> EstadoPedidos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Producto> Poductos { get; set; }
        public DbSet<ProductoColor> ProductoColores { get; set; }
        public DbSet<ProductoTalle> ProductoTalles { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Talle> Talles { get; set; }
        public DbSet<TipoProducto> TipoProductos { get; set; }
        public DbSet<TipoUsuario> TipoUsuarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        // Consultar: ¿Corresponde agregar una tabla "Direccion" donde guardar calle, barrio, loc, prov?

        public Context(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var cascadeFKs = modelBuilder.Model.G­etEntityTypes()
                                          .SelectMany(t => t.GetForeignKeys())
                                          .Where(fk => !fk.IsOwnership
                                                       && fk.DeleteBehavior == DeleteBehavior.Casca­de);


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductoColor>().HasKey(x => new { x.ProductoId, x.ColorId });
            modelBuilder.Entity<ProductoTalle>().HasKey(x => new { x.ProductoId, x.TalleId });

        }

    }

}
