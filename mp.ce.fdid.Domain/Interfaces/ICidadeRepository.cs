using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces.Repositories.Common;
using System.Collections.Generic;

namespace mp.ce.fdid.Domain.Interfaces
{
    public interface ICidadeRepository : IRepositoryBase<Cidade>
    {
        IEnumerable<Cidade> GetByIdEstado(int idEstado);
    }
}
