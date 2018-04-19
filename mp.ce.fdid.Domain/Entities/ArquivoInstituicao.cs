using System;

namespace mp.ce.fdid.Domain.Entities
{
    public class ArquivoInstituicao
    {
       public ArquivoInstituicao() {
            dDataCadastro = DateTime.Now;
        }

        public int ID { get; set; }
        public int IDInstituicao { get; set; }
        public string sNome { get; set; }
        public DateTime dDataCadastro { get; set; }

        public Instituicao Instituicao { get; set; }

    }
}
