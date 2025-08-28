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

        public Task<int> Insert(Movimiento entidad)
        {
            throw new NotImplementedException();
        }

        public Task<string?> InsertDevuelveCodigo(Movimiento entidad)
        {
            throw new NotImplementedException();
        }

        public Task<TDTO> InsertDevuelveDTO<TDTO>(Movimiento entidad)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEntidad(int id, Movimiento entidad)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEstado(int id, Movimiento entidad)
        {
            throw new NotImplementedException();
        }

        //Task<List<Movimiento>> IRepositorio<Movimiento>.Select()
        //{
        //    throw new NotImplementedException();
        //}

        //Task<Movimiento?> IRepositorio<Movimiento>.SelectByCod(string codigo)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<Movimiento?> IRepositorio<Movimiento>.SelectById(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
