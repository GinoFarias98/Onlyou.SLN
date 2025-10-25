using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onlyou.BD.Migrations
{
    /// <inheritdoc />
    public partial class DeletBehaviorRestrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movimientos_Cajas_CajaId",
                table: "Movimientos");

            migrationBuilder.DropForeignKey(
                name: "FK_Movimientos_TipoMovimientos_TipoMovimientoId",
                table: "Movimientos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_Movimientos_MovimientoId",
                table: "Pagos");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoItems_Pedidos_PedidoId",
                table: "PedidoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoItems_Productos_ProductoId",
                table: "PedidoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_EstadoPedidos_EstadoPedidoId",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductoColores_Colores_ColorId",
                table: "ProductoColores");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductoColores_Productos_ProductoId",
                table: "ProductoColores");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Categorias_CategoriaId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Marcas_MarcaId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Proveedores_ProveedorId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_TipoProductos_TipoProductoId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductoTalles_Productos_ProductoId",
                table: "ProductoTalles");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductoTalles_Talles_TalleId",
                table: "ProductoTalles");

            migrationBuilder.AddForeignKey(
                name: "FK_Movimientos_Cajas_CajaId",
                table: "Movimientos",
                column: "CajaId",
                principalTable: "Cajas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Movimientos_TipoMovimientos_TipoMovimientoId",
                table: "Movimientos",
                column: "TipoMovimientoId",
                principalTable: "TipoMovimientos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Movimientos_MovimientoId",
                table: "Pagos",
                column: "MovimientoId",
                principalTable: "Movimientos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoItems_Pedidos_PedidoId",
                table: "PedidoItems",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoItems_Productos_ProductoId",
                table: "PedidoItems",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_EstadoPedidos_EstadoPedidoId",
                table: "Pedidos",
                column: "EstadoPedidoId",
                principalTable: "EstadoPedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoColores_Colores_ColorId",
                table: "ProductoColores",
                column: "ColorId",
                principalTable: "Colores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoColores_Productos_ProductoId",
                table: "ProductoColores",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Categorias_CategoriaId",
                table: "Productos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Marcas_MarcaId",
                table: "Productos",
                column: "MarcaId",
                principalTable: "Marcas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Proveedores_ProveedorId",
                table: "Productos",
                column: "ProveedorId",
                principalTable: "Proveedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_TipoProductos_TipoProductoId",
                table: "Productos",
                column: "TipoProductoId",
                principalTable: "TipoProductos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoTalles_Productos_ProductoId",
                table: "ProductoTalles",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoTalles_Talles_TalleId",
                table: "ProductoTalles",
                column: "TalleId",
                principalTable: "Talles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movimientos_Cajas_CajaId",
                table: "Movimientos");

            migrationBuilder.DropForeignKey(
                name: "FK_Movimientos_TipoMovimientos_TipoMovimientoId",
                table: "Movimientos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_Movimientos_MovimientoId",
                table: "Pagos");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoItems_Pedidos_PedidoId",
                table: "PedidoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoItems_Productos_ProductoId",
                table: "PedidoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_EstadoPedidos_EstadoPedidoId",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductoColores_Colores_ColorId",
                table: "ProductoColores");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductoColores_Productos_ProductoId",
                table: "ProductoColores");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Categorias_CategoriaId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Marcas_MarcaId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Proveedores_ProveedorId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_TipoProductos_TipoProductoId",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductoTalles_Productos_ProductoId",
                table: "ProductoTalles");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductoTalles_Talles_TalleId",
                table: "ProductoTalles");

            migrationBuilder.AddForeignKey(
                name: "FK_Movimientos_Cajas_CajaId",
                table: "Movimientos",
                column: "CajaId",
                principalTable: "Cajas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movimientos_TipoMovimientos_TipoMovimientoId",
                table: "Movimientos",
                column: "TipoMovimientoId",
                principalTable: "TipoMovimientos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Movimientos_MovimientoId",
                table: "Pagos",
                column: "MovimientoId",
                principalTable: "Movimientos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoItems_Pedidos_PedidoId",
                table: "PedidoItems",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoItems_Productos_ProductoId",
                table: "PedidoItems",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_EstadoPedidos_EstadoPedidoId",
                table: "Pedidos",
                column: "EstadoPedidoId",
                principalTable: "EstadoPedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoColores_Colores_ColorId",
                table: "ProductoColores",
                column: "ColorId",
                principalTable: "Colores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoColores_Productos_ProductoId",
                table: "ProductoColores",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Categorias_CategoriaId",
                table: "Productos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Marcas_MarcaId",
                table: "Productos",
                column: "MarcaId",
                principalTable: "Marcas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Proveedores_ProveedorId",
                table: "Productos",
                column: "ProveedorId",
                principalTable: "Proveedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_TipoProductos_TipoProductoId",
                table: "Productos",
                column: "TipoProductoId",
                principalTable: "TipoProductos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoTalles_Productos_ProductoId",
                table: "ProductoTalles",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoTalles_Talles_TalleId",
                table: "ProductoTalles",
                column: "TalleId",
                principalTable: "Talles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
