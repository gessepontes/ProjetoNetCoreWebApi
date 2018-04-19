using System;

namespace mp.ce.fdid.Domain.Entities
{
    public class ArquivoProjeto
    {
        public ArquivoProjeto()
        {
            dDataCadastro = DateTime.Now;
        }

        public int ID { get; set; }
        public int IDProjeto { get; set; }
        public string sNome { get; set; }
        public DateTime dDataCadastro { get; set; }

        public Projeto Projeto { get; set; }

    }
}
