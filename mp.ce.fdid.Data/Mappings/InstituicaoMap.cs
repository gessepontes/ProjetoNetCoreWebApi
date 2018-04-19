using Dapper.FluentMap.Dommel.Mapping;
using mp.ce.fdid.Domain.Entities;

namespace mp.ce.fdid.Data.Mappings
{
    public class InstituicaoMap : DommelEntityMap<Instituicao>
    {
        public InstituicaoMap()
        {
            ToTable("TB_INSTITUICAO");
            Map(p => p.ID).IsKey().IsIdentity();
        }
    }
}
