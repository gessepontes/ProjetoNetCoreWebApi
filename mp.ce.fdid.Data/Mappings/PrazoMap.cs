using Dapper.FluentMap.Dommel.Mapping;
using mp.ce.fdid.Domain.Entities;

namespace mp.ce.fdid.Data.Mappings
{
    public class PrazoMap : DommelEntityMap<Prazo>
    {
        public PrazoMap()
        {
            ToTable("TB_PRAZO");
            Map(p => p.ID).IsKey().IsIdentity();
        }
    }
}
