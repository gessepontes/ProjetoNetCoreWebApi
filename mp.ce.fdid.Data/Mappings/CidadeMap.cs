using Dapper.FluentMap.Dommel.Mapping;
using mp.ce.fdid.Domain.Entities;

namespace mp.ce.fdid.Data.Mappings
{
    public class CidadeMap : DommelEntityMap<Cidade>
    {
        public CidadeMap()
        {
            ToTable("TB_CIDADE");
            Map(p => p.iCodCidade).IsKey().IsIdentity();
        }
    }
}
