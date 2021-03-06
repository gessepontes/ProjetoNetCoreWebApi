﻿using Dapper.FluentMap.Dommel.Mapping;
using mp.ce.fdid.Domain.Entities;

namespace mp.ce.fdid.Data.Mappings
{
    public class EstadoMap : DommelEntityMap<Estado>
    {
        public EstadoMap()
        {
            ToTable("TB_ESTADO");
            Map(p => p.iCodEstado).ToColumn("iCodEstado").IsKey().IsIdentity();
            Map(x => x.sNomeEstado).ToColumn("sNomeEstado");
            Map(x => x.Cidade).Ignore();
        }
    }
}
