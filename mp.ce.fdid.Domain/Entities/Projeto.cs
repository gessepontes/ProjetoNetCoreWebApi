using System;
using System.Collections.Generic;

namespace mp.ce.fdid.Domain.Entities
{
    public class Projeto
    {

        public Projeto()
        {
            dDataCadastro = DateTime.Now;
            dDataMovimentacao = DateTime.Now;
        }

        public int ID { get; set; }
        public int IDInstituicao { get; set; }
        public string sTitulo { get; set; }
        public List<int> sArea { get; set; }
        public int IDCidade { get; set; }
        public DateTime dDataInicio { get; set; }
        public DateTime dDataTermino { get; set; }
        public double mValor { get; set; }
        public double mValorContraPartida { get; set; }
        public string tResumo { get; set; }
        public string sNoProcesso { get; set; }

        public DateTime dDataCadastro { get; set; }
        public DateTime dDataMovimentacao { get; set; }

        public Cidade Cidade { get; set; }
        public virtual ICollection<ArquivoProjeto> ArquivoProjeto { get; set; }

        public Instituicao Instituicao { get; set; }
    }
}
