using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onlyou.BD.Migrations
{
    /// <inheritdoc />
    public partial class PedidoItem_ColorTalle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PedidoItems_PedidoId_ProductoId",
                table: "PedidoItems");

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "PedidoItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorNombre",
                table: "PedidoItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TalleId",
                table: "PedidoItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TalleNombre",
                table: "PedidoItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItems_ColorId",
                table: "PedidoItems",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItems_PedidoId_ProductoId",
                table: "PedidoItems",
                columns: new[] { "PedidoId", "ProductoId" });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItems_TalleId",
                table: "PedidoItems",
                column: "TalleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoItems_Colores_ColorId",
                table: "PedidoItems",
                column: "ColorId",
                principalTable: "Colores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoItems_Talles_TalleId",
                table: "PedidoItems",
                column: "TalleId",
                principalTable: "Talles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoItems_Colores_ColorId",
                table: "PedidoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoItems_Talles_TalleId",
                table: "PedidoItems");

            migrationBuilder.DropIndex(
                name: "IX_PedidoItems_ColorId",
                table: "PedidoItems");

            migrationBuilder.DropIndex(
                name: "IX_PedidoItems_PedidoId_ProductoId",
                table: "PedidoItems");

            migrationBuilder.DropIndex(
                name: "IX_PedidoItems_TalleId",
                table: "PedidoItems");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "PedidoItems");

            migrationBuilder.DropColumn(
                name: "ColorNombre",
                table: "PedidoItems");

            migrationBuilder.DropColumn(
                name: "TalleId",
                table: "PedidoItems");

            migrationBuilder.DropColumn(
                name: "TalleNombre",
                table: "PedidoItems");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItems_PedidoId_ProductoId",
                table: "PedidoItems",
                columns: new[] { "PedidoId", "ProductoId" },
                unique: true);
        }
    }
}
