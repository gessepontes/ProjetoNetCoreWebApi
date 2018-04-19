using Dapper;
using mp.ce.fdid.Data.Repositories.Common;
using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces;
using System.Linq;

namespace mp.ce.fdid.Data.Repositories
{
    public class PrazoRepository : RepositoryBase<Prazo>, IPrazoRepository
    {
        public bool Prazo()
        {
            var prazo = conn.Query<Prazo>("select top(1) * from TB_PRAZO WHERE GETDATE() BETWEEN dDataInicio AND dDataTermino").FirstOrDefault();

            if (prazo != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
