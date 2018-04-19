using Dapper.FluentMap.Dommel.Mapping;
using mp.ce.fdid.Domain.Entities;

namespace mp.ce.fdid.Data.Mappings
{
    public class ProjetoMap : DommelEntityMap<Projeto>
    {
        public ProjetoMap()
        {
            ToTable("TB_PROJETOS");
            Map(p => p.ID).IsKey().IsIdentity();
        }
    }
}
