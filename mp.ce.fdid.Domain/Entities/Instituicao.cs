using System;
using System.Collections.Generic;

namespace mp.ce.fdid.Domain.Entities
{
    public class Instituicao
    {

        public Instituicao()
        {
            dDataCadastro = DateTime.Now;
            dDataMovimentacao = DateTime.Now;
        }

        public int ID { get; set; }
        public string sProponente { get; set; }
        public string sCNPJ { get; set; }
        public string sEndereco { get; set; }
        public string sCep { get; set; }
        public string sTelefone { get; set; }
        public string sFax { get; set; }
        public int IDCidade { get; set; }
        public string sEmail { get; set; }
        public string sHomePage { get; set; }
        public int iRegime { get; set; }
        public int iEsfera { get; set; }
        public string sRepresentante { get; set; }
        public string sCpfRepresentante { get; set; }
        public string sCargo { get; set; }
        public string sFuncao { get; set; }
        public string sRG { get; set; }
        public string sOrgaoExpedidor { get; set; }
        public string sEnderecoRepresentante { get; set; }
        public string sTelefoneRepresentante { get; set; }
        public string sCepRepresentante { get; set; }
        public string sCoordenador { get; set; }
        public string sCPFCoordenador { get; set; }
        public string sTelefoneCoordenador { get; set; }
        public string sFaxCoordenador { get; set; }
        public char cPerfil { get; set; }

        string SENHA;
        public string sSenha
        {
            get
            {
                if (string.IsNullOrEmpty(SENHA))
                {
                    return SENHA;
                }
                return Diversos.Diversos.GenerateMD5(SENHA);
            }
            set
            {
                SENHA = value;
            }

        }

        public DateTime dDataCadastro { get; set; }
        public DateTime dDataMovimentacao { get; set; }

        public Cidade Cidade { get; set; }

        public int IDEstado { get; set; }
        public virtual ICollection<Arquivo> Arquivo { get; set; }
        public virtual ICollection<Projeto> Projeto { get; set; }

    }
}
