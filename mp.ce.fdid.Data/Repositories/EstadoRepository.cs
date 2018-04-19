using Dapper;
using mp.ce.fdid.Data.Repositories.Common;
using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces;
using System.Linq;

namespace mp.ce.fdid.Data.Repositories
{
    public class EstadoRepository : RepositoryBase<Estado>, IEstadoRepository
    {
        public int GetIdEstato(int idCidade) => conn.Query<int>("SELECT iCodEstado from TB_CIDADE WHERE iCodCidade = @idCidade ", new { idCidade }).FirstOrDefault();
    }
}
