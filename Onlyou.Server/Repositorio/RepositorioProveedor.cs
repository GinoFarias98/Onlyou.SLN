using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioProveedor : Repositorio<Proveedor>, IRepositorioProveedor
    {
        private readonly Context context;

        public RepositorioProveedor(Context context) : base(context)
        {
            this.context = context;
        }

        public async Task<Proveedor?> SelectByCUIT(string cuit)
        {
            try
            {
                Proveedor? proveedorxCUIT = await context.Proveedores.SingleOrDefaultAsync(x => x.CUIT == cuit);
                return proveedorxCUIT;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        public async Task<Proveedor?> SelectByRS(string rs)
        {
            try
            {
                Proveedor? proveedorxRS = await context.Proveedores.FirstOrDefaultAsync(x => x.RazonSocial == rs);
                return proveedorxRS;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

    }
}
