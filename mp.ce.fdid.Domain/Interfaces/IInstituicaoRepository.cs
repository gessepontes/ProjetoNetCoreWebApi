using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces.Repositories.Common;

namespace mp.ce.fdid.Domain.Interfaces
{
    public interface IInstituicaoRepository : IRepositoryBase<Instituicao>
    {
        Instituicao GetByIdInstituicao(int GetByIdInstituicao);
        Instituicao GetAuthenticate(string cnpj, string password);
        Auth GetHash(string hash);
        int GetSend(string cnpj);
        int GetChangerPassword(string cnpj, string password);
    }
}
