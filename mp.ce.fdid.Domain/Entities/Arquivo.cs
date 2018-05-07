using System;

namespace mp.ce.fdid.Domain.Entities
{
    public class Arquivo
    {
        public Arquivo()
        {
            dDataCadastro = DateTime.Now;
        }

        public int ID { get; set; }
        public int IDInstituicaoProjeto { get; set; }
        public int iTipo { get; set; }
        public string sNome { get; set; }
        public string sNomebase { get; set; }
        public DateTime dDataCadastro { get; set; }
    }
}
