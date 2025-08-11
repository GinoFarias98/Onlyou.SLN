using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(Nombre), IsUnique = true)] // Índice único en Nombre
    public class EstadoPedido : EntidadBase
    {
        // Nombre -------------------------------------------------------------------

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(128, ErrorMessage = "Máximo {1} caracteres.")]
        [Display(Name = "Nombre",
                 Description = "Nombre único del estado del pedido.")]
        public string Nombre { get; set; } = null!;

        // Pedidos relacionados ----------------------------------------------------

        [Display(Name = "Pedidos",
                 Description = "Lista de pedidos asociados a este estado.")]
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
