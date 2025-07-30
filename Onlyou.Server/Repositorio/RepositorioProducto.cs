using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioProducto : Repositorio<Producto>
    {
        private readonly Context context;

        public RepositorioProducto(Context context) : base(context)
        {
            this.context = context;
        }



    }
}
