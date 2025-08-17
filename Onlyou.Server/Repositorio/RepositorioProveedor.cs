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

        public async Task<List<Proveedor>> SelectByEmail(string email)
        {
            try
            {
                var proveedoresByEmail = await context.Proveedores.Where(prov => EF.Functions.Like(prov.Email, $"%{email}")).ToListAsync();
                return proveedoresByEmail;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        public async Task<List<Proveedor>> SelectBySimilName(string similName)
        {
            try
            {
                var ProveedoresByName = await context.Proveedores.Where(prov => EF.Functions.Like(prov.Nombre, $"{similName.ToLower()}")).ToListAsync();
                return ProveedoresByName;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

        public async Task<Proveedor?> SelectByCuit(string cuit)
        {
            try
            {
                var proveedorByCuit = await context.Proveedores.FirstOrDefaultAsync(p => p.CUIT == cuit);
                return proveedorByCuit;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

        public async Task<bool> ExisteProv(string cuit, string email)
        {
            try
            {
                bool ExisteProv = await context.Proveedores.AnyAsync(p => p.CUIT == cuit || p.Email == email);
                return ExisteProv;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);

                throw;
            }

        }

    }
}
