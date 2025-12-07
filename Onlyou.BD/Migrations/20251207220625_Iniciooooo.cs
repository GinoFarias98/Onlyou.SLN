using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onlyou.BD.Migrations
{
    /// <inheritdoc />
    public partial class Iniciooooo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SaldoActual",
                table: "Cajas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaldoActual",
                table: "Cajas");
        }
    }
}
