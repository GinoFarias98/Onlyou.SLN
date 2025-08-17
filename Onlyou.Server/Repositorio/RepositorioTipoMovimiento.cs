using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioTipoMovimiento : Repositorio<TipoMovimiento>, IRepositorioTipoMovimiento
    {
        private readonly Context context;
        public RepositorioTipoMovimiento(Context context) : base(context)
        {
            this.context = context;
        }

    }
}
