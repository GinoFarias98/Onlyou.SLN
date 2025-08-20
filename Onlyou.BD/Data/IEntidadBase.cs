using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data
{
    public interface IEntidadBase
    {
        int Id { get; set; }
        string Codigo { get; set; } 
        bool Estado { get; set; } 
    } 
}
