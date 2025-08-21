using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioTipoMovimiento : Repositorio<TipoMovimiento>, IRepositorioTipoMovimiento
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public RepositorioTipoMovimiento(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

    }
}
