using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.Enums
{
    public enum EstadoMovimientoDto
    {
    
        Pendiente,   // El movimiento fue creado pero no se concretó
        Pagado,      // Ya se realizó el pago o ingreso correspondiente
        Anulado,     // Movimiento cancelado
        Parcial,     // Opcional, si se admite pago parcial
        Rechazado    // Opcional, para movimientos que no se validan
        
    }
}
