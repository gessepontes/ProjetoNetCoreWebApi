using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces.Repositories.Common;

namespace mp.ce.fdid.Domain.Interfaces
{
    public interface IEstadoRepository : IRepositoryBase<Estado>
    {
        int GetIdEstato(int idCidade);
    }
}
