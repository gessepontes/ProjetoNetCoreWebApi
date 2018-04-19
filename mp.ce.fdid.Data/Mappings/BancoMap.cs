using Dapper.FluentMap.Dommel.Mapping;
using mp.ce.fdid.Domain.Entities;

namespace mp.ce.fdid.Data.Mappings
{
    public class BancoMap : DommelEntityMap<Banco>
    {
        public BancoMap()
        {
            ToTable("TB_BANCO");
            Map(p => p.iCodBanco).IsKey().IsIdentity();
        }
    }
}
