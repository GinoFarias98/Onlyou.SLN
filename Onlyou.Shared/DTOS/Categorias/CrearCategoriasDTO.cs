﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Categorias
{
    public class CrearCategoriasDTO
    {
            public int Id { get; set; }
            public string? Codigo { get; set; }
            public string? Nombre { get; set; }
            public bool Estado { get; set; }
    }
}
