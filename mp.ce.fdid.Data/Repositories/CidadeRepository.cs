using Dapper;
using mp.ce.fdid.Data.Repositories.Common;
using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace mp.ce.fdid.Data.Repositories
{
    public class CidadeRepository : RepositoryBase<Cidade>, ICidadeRepository
    {
        public IEnumerable<Cidade> GetByIdEstado(int idEstado) => conn.Query<Cidade>("select * from TB_CIDADE WHERE iCodEstado = @iCodEstado ORDER BY sNomeCidade", new { iCodEstado = idEstado }).ToList();
}
}
