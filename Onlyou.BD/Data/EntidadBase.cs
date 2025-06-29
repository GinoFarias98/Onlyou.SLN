using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data
{
    public class EntidadBase : IEntidadBase
    {   
        public int Id { get; set; }
        public string? Codigo { get; set; }
    }
}
