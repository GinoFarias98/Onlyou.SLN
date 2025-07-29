using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(UserName), IsUnique = true)] // Evita duplicación de usuarios
    public class Usuario : EntidadBase
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [MaxLength(64, ErrorMessage = "El nombre de usuario no debe superar los 64 caracteres.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MaxLength(128, ErrorMessage = "La contraseña no debe superar los 128 caracteres.")]
        public string Contrasenia { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Tipo de Usuario.")]
        public int TipoUsuarioId { get; set; }
        public TipoUsuario TipoUsuario { get; set; } = null!;
    }
}
