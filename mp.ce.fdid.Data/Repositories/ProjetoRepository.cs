using Dapper;
using Microsoft.Extensions.Configuration;
using mp.ce.fdid.Data.Repositories.Common;
using mp.ce.fdid.Domain.Diversos;
using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace mp.ce.fdid.Data.Repositories
{
    public class ProjetoRepository : RepositoryBase<Projeto>, IProjetoRepository
    {

        public int AddProjeto(Projeto obj)
        {
            string sql = "";
            conn.Open();

            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    var IDProcesso = conn.Query<int>("PROTOCOLO..spCriarProcesso", new
                    {
                        iNoDocumento = 0,
                        iAnoDocumento = DateTime.Now.Year,
                        iCodUnidadeOrigem = 854,
                        iCodUnidadeDestino = 63,
                        iCodMotivo = 1,
                        iCodAssunto = 0,
                        sDescricaoAssunto = "FDID Inscrição de Projeto-" + DateTime.Now.Month + "/" + DateTime.Now.Year,
                        iCodCidade = 255,
                        sNoSpu = "",
                        sConclusao = "",
                        iCodUsuarioRemetente = 1,
                        codFila = 0,
                        codClasse = 910020
                    }, commandType: CommandType.StoredProcedure, transaction: transaction).SingleOrDefault();

                    obj.sNoProcesso = conn.Query<string>("select top(1) CONVERT(varchar(10), iNoProcesso) + '/' + CONVERT(varchar(10), iAnoProcesso) + '-' + CONVERT(varchar(10), iDigito) from PROTOCOLO..tblProcessos WHERE iCodigo =@iCodigo", new { iCodigo = IDProcesso.ToString() }, transaction: transaction).FirstOrDefault();

                    sql = "INSERT INTO TB_PROJETOS(IDInstituicao,sTitulo,IDCidade,dDataInicio,dDataTermino,mValor,mValorContraPartida,tResumo,sNoProcesso,dDataCadastro,dDataMovimentacao) ";
                    sql = sql + "values(@IDInstituicao,@sTitulo,@IDCidade,@dDataInicio,@dDataTermino,@mValor,@mValorContraPartida,@tResumo,@sNoProcesso,@dDataCadastro,@dDataMovimentacao); SELECT CAST(SCOPE_IDENTITY() as int)";

                    var returnId = conn.Query<int>(sql, new { obj.IDInstituicao, obj.sTitulo, obj.IDCidade, obj.dDataInicio, obj.dDataTermino, obj.mValor, obj.mValorContraPartida, obj.tResumo, obj.sNoProcesso, obj.dDataCadastro, obj.dDataMovimentacao }, transaction: transaction).SingleOrDefault();

                    if (obj.sArea != null)
                    {
                        if (returnId != 0 && obj.sArea.Count() > 0)
                        {
                            foreach (var item in obj.sArea)
                            {
                                conn.Execute(@"INSERT TB_AREAPROJETO(IDProjeto,iArea) values(@IDProjeto,@iArea)", new { IDProjeto = returnId, iArea = item }, transaction: transaction);
                            }

                        }
                    }

                    transaction.Commit();

                    return returnId;
                }
                catch (System.Exception e)
                {
                    e.Message.ToString();
                    transaction.Rollback();
                    return 0;
                }

            }


        }

        public bool UpdateProjeto(Projeto obj)
        {
            string sql = "UPDATE TB_PROJETOS SET IDInstituicao=@IDInstituicao,sTitulo=@sTitulo,IDCidade=@IDCidade,dDataInicio=@dDataInicio,dDataTermino=@dDataTermino,mValor=@mValor," +
                " mValorContraPartida=@mValorContraPartida,tResumo=@tResumo,dDataMovimentacao=GETDATE() WHERE ID = @ID; ";

            conn.Open();

            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    conn.Execute(sql, new
                    {
                        obj.IDInstituicao,
                        obj.sTitulo,
                        obj.sArea,
                        obj.IDCidade,
                        obj.dDataInicio,
                        obj.dDataTermino,
                        obj.mValor,
                        obj.mValorContraPartida,
                        obj.tResumo,
                        obj.sNoProcesso,
                        obj.ID
                    }, transaction: transaction);

                    conn.Execute("DELETE TB_AREAPROJETO WHERE IDProjeto = @IDProjeto; ", new { IDProjeto = obj.ID }, transaction: transaction);

                    if (obj.sArea != null)
                    {
                        foreach (var item in obj.sArea)
                        {
                            conn.Execute(@"INSERT TB_AREAPROJETO(IDProjeto,iArea) values(@IDProjeto,@iArea)", new { IDProjeto = obj.ID, iArea = item }, transaction: transaction);
                        }

                    }

                    transaction.Commit();
                    return true;
                }
                catch (System.Exception e)
                {
                    e.Message.ToString();
                    transaction.Rollback();
                    return false;
                }

            }
        }

        public bool RemoveProjeto(Projeto obj)
        {
            conn.Open();

            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    conn.Execute("DELETE TB_AREAPROJETO WHERE IDProjeto = @IDProjeto; ", new { IDProjeto = obj.ID }, transaction: transaction);
                    conn.Execute("DELETE TB_ARQUIVO WHERE IDInstituicaoProjeto = @IDProjeto AND iTipo = 2; ", new { IDProjeto = obj.ID }, transaction: transaction);
                    conn.Execute("DELETE TB_PROJETOS WHERE ID = @ID; ", new { obj.ID }, transaction: transaction);

                    transaction.Commit();
                    return true;
                }
                catch (System.Exception e)
                {
                    e.Message.ToString();
                    transaction.Rollback();
                    return false;
                }

            }
        }


        public override Projeto GetById(int? id)
        {
            var instituicaoDictionary = new Dictionary<int, Projeto>();

            var list = conn.Query<Projeto, Instituicao, Arquivo, Projeto>(
                @"SELECT * FROM TB_PROJETOS P INNER JOIN TB_INSTITUICAO I ON P.IDInstituicao = I.ID LEFT JOIN TB_ARQUIVO A ON P.ID = A.IDInstituicaoProjeto WHERE P.ID = @id",
                map: (projeto, instituicao, arquivoProjeto) =>
                {
                    Projeto projetoEntry;

                    if (!instituicaoDictionary.TryGetValue(projeto.ID, out projetoEntry))
                    {
                        projetoEntry = projeto;
                        projetoEntry.Arquivo = new List<Arquivo>();
                        instituicaoDictionary.Add(projetoEntry.ID, projetoEntry);
                    }

                    projetoEntry.Instituicao = instituicao;

                    projetoEntry.Arquivo.Add(arquivoProjeto);
                    return projetoEntry;
                },
                splitOn: "ID",
                param: new { id }).Distinct().FirstOrDefault();

            List<int> termsList = new List<int>();

            if (list != null)
            {
                foreach (var item in conn.Query("select iArea from TB_AREAPROJETO WHERE IDProjeto =@IDProjeto", new { IDProjeto = list.ID }).ToList())
                {
                    termsList.Add(item.iArea);
                }

                list.sArea = termsList;
            }

            return list;
        }

        public IEnumerable<Projeto> GetByIdIsntituicao(int id) => conn.Query<Projeto>(@"SELECT * FROM TB_PROJETOS P WHERE P.idInstituicao = @id", new { id }).ToList();

        public void SendEmail(int IDProjeto, int iTipo)
        {
            string _body = "";
            string strBody = "";

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Projeto _projeto = GetById(IDProjeto);

            _body = "<tr bgcolor='#D6EFDA'><td colspan=2  style='font-weight:bold'>INSTITUIÇÃO</td></tr>";
            _body += "<tr style='=font-weight:bold;'>";
            _body += "<td font-weight:bold'><b>Proponente: </b>" + _projeto.Instituicao.sProponente + "</td>";
            _body += "<td font-weight:bold'><b>CNPJ: </b>" + _projeto.Instituicao.sCNPJ + "</td></tr>";

            _body += "<tr style='=font-weight:bold;'>";
            _body += "<td font-weight:bold'><b>Endereço: </b>" + _projeto.Instituicao.sEndereco + "</td>";
            _body += "<td font-weight:bold'><b>CEP: </b>" + _projeto.Instituicao.sCep + "</td></tr>";

            _body += "<tr style='=font-weight:bold;'>";
            _body += "<td font-weight:bold'><b>Telefone: </b>" + _projeto.Instituicao.sTelefone + "</td>";
            _body += "<td font-weight:bold'<b>Fax: </b>" + _projeto.Instituicao.sFax + "</td></tr>";

            _body += "<tr style='=font-weight:bold;'>";
            _body += "<td font-weight:bold' colspan=2><b>Municipio: </b>" + _projeto.Instituicao.IDCidade + "</td></tr>";

            _body += "<tr style='=font-weight:bold;'>";
            _body += "<td font-weight:bold'<b>Email: </b>" + _projeto.Instituicao.sEmail + "</td>";
            _body += "<td font-weight:bold'><b>HomePage: </b>" + _projeto.Instituicao.sHomePage + "</td></tr>";

            _body += "<tr style='=font-weight:bold;'>";

            switch (_projeto.Instituicao.iRegime)
            {
                case 1:
                    _body += "<td font-weight:bold'<b>Regime: </b>Direito Público</td>";

                    break;
                case 2:
                    _body += "<td font-weight:bold'<b>Regime: </b>Direito Privado</td>";

                    break;
            }

            switch (_projeto.Instituicao.iEsfera)
            {
                case 1:
                    _body += "<td font-weight:bold'<b>Esfera: </b>Federal</td></tr>";
                    break;
                case 2:
                    _body += "<td font-weight:bold'<b>Esfera: </b>Estadual</td></tr>";
                    break;
                case 3:
                    _body += "<td font-weight:bold'<b>Esfera: </b>Municipal</td></tr>";
                    break;
                case 4:
                    _body += "<td font-weight:bold'<b>Esfera: </b>Organização Ambientalista</td></tr>";
                    break;
                case 5:
                    _body += "<td font-weight:bold'<b>Esfera: </b>Outros</td></tr>";
                    break;
            }

            _body += "<tr bgcolor='#D6EFDA'><td colspan=2  style='font-weight:bold'>REPRESENTANTE</td></tr>";

            _body += "<td font-weight:bold'><b>Representante: </b>" + _projeto.Instituicao.sRepresentante + "</td>";
            _body += "<td font-weight:bold'><b>Cpf: </b>" + _projeto.Instituicao.sCpfRepresentante + "</td>";
            _body += "<tr><td font-weight:bold'><b>Cargo: </b>" + _projeto.Instituicao.sCargo + "</td>";
            _body += "<td font-weight:bold'><b>Função: </b>" + _projeto.Instituicao.sFuncao + "</td></tr>";
            _body += "<tr><td font-weight:bold'><b>Rg: </b>" + _projeto.Instituicao.sRG + "</td>";
            _body += "<td font-weight:bold'><b>Órgão Expedidor: </b>" + _projeto.Instituicao.sOrgaoExpedidor + "</td></tr>";
            _body += "<tr><td font-weight:bold' colspan=2><b>Endereço Representante: </b>" + _projeto.Instituicao.sEnderecoRepresentante + "</td></tr>";
            _body += "<tr><td font-weight:bold'><b>Telefone: </b>" + _projeto.Instituicao.sTelefoneRepresentante + "</td>";
            _body += "<td font-weight:bold'><b>CEP: </b>" + _projeto.Instituicao.sCepRepresentante + "</td></tr>";

            _body += "<tr bgcolor='#D6EFDA'><td colspan=2  style='font-weight:bold'>COORDENADOR</td></tr>";
            _body += "<tr><td font-weight:bold'><b>Coordenador: </b>" + _projeto.Instituicao.sCoordenador + "</td>";
            _body += "<td font-weight:bold'><b>Cpf: </b>" + _projeto.Instituicao.sCPFCoordenador + "</td></tr>";
            _body += "<tr><td font-weight:bold'><b>Telefone: </b>" + _projeto.Instituicao.sTelefoneCoordenador + "</TD>";
            _body += "<td font-weight:bold'><b>Fax: </b>" + _projeto.Instituicao.sFaxCoordenador + "</td></tr>";

            _body += "<tr bgcolor='#D6EFDA'><td colspan=2  style='font-weight:bold'>PROJETO</td></tr>";

            _body += "<tr><td font-weight:bold' colspan=2><b>Título: </b>" + _projeto.sTitulo + "</td>";

            _body += "<tr><td font-weight:bold' colspan=2><b>Area </b></td></tr>";

            foreach (int item in _projeto.sArea)
            {
                switch (item)
                {
                    case 1:
                        _body += "<tr><td font-weight:bold' colspan=2>Meio Ambiente</td></tr>";
                        break;
                    case 2:
                        _body += "<tr><td font-weight:bold' colspan=2>Consumidor</td></tr>";
                        break;
                    case 3:
                        _body += "<tr><td font-weight:bold' colspan=2>Defesa da Concorrencia</td></tr>";
                        break;
                    case 4:
                        _body += "<tr><td font-weight:bold' colspan=2>Artístico</td></tr>";
                        break;
                    case 5:
                        _body += "<tr><td font-weight:bold' colspan=2>Estítico</td></tr>";
                        break;
                    case 6:
                        _body += "<tr><td font-weight:bold' colspan=2>Histórico</td></tr>";
                        break;
                    case 7:
                        _body += "<tr><td font-weight:bold' colspan=2>Turístico</td></tr>";
                        break;
                    case 8:
                        _body += "<tr><td font-weight:bold' colspan=2>Paisagismo</td></tr>";
                        break;
                    case 9:
                        _body += "<tr><td font-weight:bold' colspan=2>Direitos Difusos</td></tr>";
                        break;
                    case 10:
                        _body += "<tr><td font-weight:bold' colspan=2>Reaparelhamento e Modernização</td></tr>";
                        break;
                }
            }

            _body += "<tr><td font-weight:bold'><b>Município: </b>" + _projeto.IDCidade + "</td>";
            _body += "<tr><b>Número do Processo: </b>" + _projeto.sNoProcesso + "</td></tr>";
            _body += "<tr><td font-weight:bold'><b>Início: </b>" + _projeto.dDataInicio + "</td>";
            _body += "<td font-weight:bold'><b>Termino: </b>" + _projeto.dDataTermino + "</td></tr>";
            _body += "<tr><td font-weight:bold'><b>Valor do Projeto: </b>" + _projeto.mValor + "</td>";
            _body += "<td font-weight:bold'><b>Valor da Contrapartida: </b>" + _projeto.mValorContraPartida + "</td></tr>";
            _body += "<tr><td font-weight:bold' colspan=2><b>Resumo: </b>" + _projeto.tResumo + "</td></tr>";

            List<string> _anexos = new List<string>();

            foreach (Arquivo item in _projeto.Arquivo)
            {
                if (item != null)
                {
                    _anexos.Add(Diversos.PathArquivo(item.sNomebase, "PROJETO"));
                }

            }

            strBody = "";
            strBody = strBody + "<html>";
            strBody = strBody + "<head>";
            strBody = strBody + "<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>";
            strBody = strBody + "<title>Untitled Document</title>";
            strBody = strBody + "</head>";
            strBody = strBody + "<body>";

            strBody = strBody + "<table style='font-family: verdana; font-size: 11px; color: #000000;' width='100%'>";
            strBody = strBody + "<tr align=center><td colspan=2><img src='cid:Imagem1' /></td></tr>";
            strBody = strBody + "<tr align=center><td colspan=2></td></tr>";
            strBody = strBody + "<tr style='=font-weight:bold;' align=center><td colspan=2>PROCURADORIA GERAL DE JUSTIÇA</td></tr>";
            strBody = strBody + "<tr style='=font-weight:bold;' align=center><td colspan=2>MINISTÉRIO PÚBLICO DO ESTADO DO CEARÁ</td></tr>";

            strBody = strBody + "<tr><td font-weight:bold'><p><p></td></tr> ";
            strBody = strBody + _body;
            strBody = strBody + "</table> ";
            strBody = strBody + "<br><br>";
            strBody = strBody + "<a href='" + config.GetSection(key: "Config")["sSite"]  + "'>Clique aqui para visualizar o projeto na íntegra, incluindo arquivos anexados!<a>";
            strBody = strBody + "<br><br>";
            strBody = strBody + "Esta é uma  mensagem automática enviada pelo sistema. Não precisa responder.";
            strBody = strBody + "</body>";
            strBody = strBody + "</html>";

            string sTitulo;

            if (iTipo == 1)
            {
                sTitulo = "[PROJETOS FDID] Novo projeto cadastrado";
            }
            else
            {
                sTitulo = "Atualização do Projetos FDID";
            }

            Diversos.SendEmail(config.GetSection(key: "Config")["sEmailSend"], sTitulo, strBody, _anexos, _projeto.Instituicao.sEmail);
        }

        public override IEnumerable<Projeto> GetAll() => conn.Query<Projeto>("SELECT * FROM TB_PROJETOS").ToList();
    }
}
