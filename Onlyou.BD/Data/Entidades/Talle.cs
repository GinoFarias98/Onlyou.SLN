﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    public class Talle : EntidadBase
    {
        public string? Nombre { get; set; }

        public ICollection<ProductoTalle> ProductosTalles = new List<ProductoTalle>();
    }
}
