using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using mp.ce.fdid.Data.Mappings;

namespace mp.ce.fdid.Data.Repositories.Common
{
    public static class RegisterMappings
    {
        public static void Register()
        {
            FluentMapper.Initialize(c =>
            {
                c.AddMap(mapper: new EstadoMap());
                c.AddMap(mapper: new CidadeMap());
                c.AddMap(mapper: new InstituicaoMap());
                c.AddMap(mapper: new ProjetoMap());
                c.AddMap(mapper: new ArquivoMap());
                c.AddMap(mapper: new PrazoMap());

                c.ForDommel();
            });
        }
    }
}