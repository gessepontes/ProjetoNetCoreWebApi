
using System.Collections.Generic;

namespace mp.ce.fdid.Domain.Entities
{
    public class Estado
    {
        public int iCodEstado { get; set; }
        public string sNomeEstado { get; set; }

        public virtual ICollection<Cidade> Cidade { get; set; }

    }
}
