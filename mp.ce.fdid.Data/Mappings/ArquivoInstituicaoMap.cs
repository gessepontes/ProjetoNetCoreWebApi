using Dapper.FluentMap.Dommel.Mapping;
using mp.ce.fdid.Domain.Entities;

namespace mp.ce.fdid.Data.Mappings
{
    public class ArquivoInstituicaoMap : DommelEntityMap<ArquivoInstituicao>
    {
        public ArquivoInstituicaoMap()
        {
            ToTable("TB_ARQUIVOINSTITUICAO");
            Map(p => p.ID).IsKey().IsIdentity();
        }
    }
}
