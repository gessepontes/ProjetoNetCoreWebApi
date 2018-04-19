using Dapper.FluentMap.Dommel.Mapping;
using mp.ce.fdid.Domain.Entities;

namespace mp.ce.fdid.Data.Mappings
{
    public class ArquivoProjetoMap : DommelEntityMap<ArquivoProjeto>
    {
        public ArquivoProjetoMap()
        {
            ToTable("TB_ARQUIVO");
            Map(p => p.ID).IsKey().IsIdentity();
        }
    }
}
