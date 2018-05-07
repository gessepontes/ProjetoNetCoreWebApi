using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces.Repositories.Common;
using System.Collections.Generic;

namespace mp.ce.fdid.Domain.Interfaces
{
    public interface IProjetoRepository : IRepositoryBase<Projeto>
    {
        int AddProjeto(Projeto obj);
        bool UpdateProjeto(Projeto obj);
        bool RemoveProjeto(Projeto obj);
        void SendEmail(int IDInstituicaoProjeto, int iTipo);
        IEnumerable<Projeto> GetByIdIsntituicao(int id);
    }
}
