using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces.Repositories.Common;

namespace mp.ce.fdid.Domain.Interfaces
{
    public interface IProjetoRepository : IRepositoryBase<Projeto>
    {
        int AddProjeto(Projeto obj);
        bool UpdateProjeto(Projeto obj);
        bool RemoveProjeto(Projeto obj);
        void SendEmail(int IDProjeto, int iTipo);
    }
}
