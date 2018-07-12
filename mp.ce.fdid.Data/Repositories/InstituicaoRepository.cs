using Dapper;
using mp.ce.fdid.Data.Repositories.Common;
using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces;
using mp.ce.fdid.Domain.Diversos;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace mp.ce.fdid.Data.Repositories
{
    public class InstituicaoRepository : RepositoryBase<Instituicao>, IInstituicaoRepository
    {

        public int AddInstituicao(Instituicao obj)
        {
            string sql = "";

            sql = "INSERT INTO TB_INSTITUICAO VALUES (@sProponente,@sCNPJ,@sEndereco,@sCep,@sTelefone,@sFax,@IDCidade,@sEmail,@sHomePage,@iRegime,@iEsfera,@iNatureza,@sRepresentante,@sCpfRepresentante," +
                "@sCargo,@sFuncao,@sRG,@sOrgaoExpedidor,@sEnderecoRepresentante,@sTelefoneRepresentante,@sCepRepresentante,@sCoordenador," +
                "@sCPFCoordenador,@sTelefoneCoordenador,@sFaxCoordenador,getdate(),@sSenha,getdate(),'',null,'U',@sCelularCoordenador,@sCelularRepresentante) SELECT CAST(SCOPE_IDENTITY() as int)";

            return conn.Query<int>(sql, new
            {
                obj.sProponente,
                obj.sCNPJ,
                obj.sEndereco,
                obj.sCep,
                obj.sTelefone,
                obj.sFax,
                obj.IDCidade,
                obj.sEmail,
                obj.sHomePage,
                obj.iRegime,
                obj.iEsfera,
                obj.iNatureza,
                obj.sRepresentante,
                obj.sCpfRepresentante,
                obj.sCargo,
                obj.sFuncao,
                obj.sRG,
                obj.sOrgaoExpedidor,
                obj.sEnderecoRepresentante,
                obj.sTelefoneRepresentante,
                obj.sCepRepresentante,
                obj.sCoordenador,
                obj.sCPFCoordenador,
                obj.sTelefoneCoordenador,
                obj.sFaxCoordenador,
                obj.sSenha,
                obj.ID,
                obj.sCelularCoordenador,
                obj.sCelularRepresentante
            }).SingleOrDefault(); 
        }


        public override void Update(Instituicao obj)
        {
            string sql = "";
            string parametros = "";

            if (obj.sSenha != "") parametros = ",sSenha=@sSenha";

            sql = "UPDATE TB_INSTITUICAO SET sProponente=@sProponente,sCNPJ=@sCNPJ,sEndereco=@sEndereco,sCep=@sCep,sTelefone=@sTelefone,sFax=@sFax,IDCidade=@IDCidade,sEmail=@sEmail,sHomePage=@sHomePage,iRegime=@iRegime,iEsfera=@iEsfera,iNatureza=@iNatureza,sRepresentante=@sRepresentante,sCpfRepresentante=@sCpfRepresentante," +
                "sCargo=@sCargo,sFuncao=@sFuncao,sRG=@sRG,sOrgaoExpedidor=@sOrgaoExpedidor,sEnderecoRepresentante=@sEnderecoRepresentante,sTelefoneRepresentante=@sTelefoneRepresentante,sCepRepresentante=@sCepRepresentante,sCoordenador=@sCoordenador," +
                "sCPFCoordenador=@sCPFCoordenador,sTelefoneCoordenador=@sTelefoneCoordenador,sFaxCoordenador=@sFaxCoordenador,dDataMovimentacao=getdate(),sCelularCoordenador=@sCelularCoordenador,sCelularRepresentante=@sCelularRepresentante " + parametros + " WHERE ID = @ID; ";

            conn.Execute(sql, new
            {
                obj.sProponente,
                obj.sCNPJ,
                obj.sEndereco,
                obj.sCep,
                obj.sTelefone,
                obj.sFax,
                obj.IDCidade,
                obj.sEmail,
                obj.sHomePage,
                obj.iRegime,
                obj.iEsfera,
                obj.iNatureza,
                obj.sRepresentante,
                obj.sCpfRepresentante,
                obj.sCargo,
                obj.sFuncao,
                obj.sRG,
                obj.sOrgaoExpedidor,
                obj.sEnderecoRepresentante,
                obj.sTelefoneRepresentante,
                obj.sCepRepresentante,
                obj.sCoordenador,
                obj.sCPFCoordenador,
                obj.sTelefoneCoordenador,
                obj.sFaxCoordenador,
                obj.sSenha,
                obj.ID,
                obj.sCelularCoordenador,
                obj.sCelularRepresentante
            });
        }

        public override Instituicao GetById(int? id)
        {

            var instituicaoDictionary = new Dictionary<int, Instituicao>();

            var list = conn.Query<Instituicao, Arquivo, Instituicao>(
                @"SELECT C.*,CI.iCodEstado IDEstado,T.* FROM TB_INSTITUICAO C INNER JOIN TB_CIDADE CI ON CI.iCodCidade = C.IDCidade  LEFT JOIN TB_ARQUIVO T ON C.ID = T.IDInstituicaoProjeto  AND T.iTipo=1 WHERE C.ID= @id",
                map: (instituicao, arquivoInstituicao) =>
                {
                    Instituicao instituicaoEntry;

                    if (!instituicaoDictionary.TryGetValue(instituicao.ID, out instituicaoEntry))
                    {
                        instituicaoEntry = instituicao;
                        instituicaoEntry.Arquivo = new List<Arquivo>();
                        instituicaoDictionary.Add(instituicaoEntry.ID, instituicaoEntry);
                    }

                    instituicaoEntry.Arquivo.Add(arquivoInstituicao);
                    return instituicaoEntry;
                },
                splitOn: "ID",
                param: new { id }).Distinct().FirstOrDefault();

            return list;
        }

        public bool RemoveInstituicao(Instituicao obj)
        {
            conn.Open();

            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    conn.Execute("DELETE TB_ARQUIVO WHERE IDInstituicaoProjeto = @IDProjeto AND iTipo = 1; ", new { IDProjeto = obj.ID }, transaction: transaction);
                    conn.Execute("DELETE TB_INSTITUICAO WHERE ID = @ID; ", new { obj.ID }, transaction: transaction);

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


        public Instituicao GetByIdInstituicao(int IdInstituicao)
        {
            Instituicao i = GetById(IdInstituicao);
            i.Arquivo = conn.Query<Arquivo>("SELECT * FROM TB_ARQUIVO WHERE IDInstituicaoProjeto =@IDInstituicao AND iTipo=1", new { IDInstituicao = IdInstituicao }).ToList();
            return i;
        }

        //public Instituicao GetAuthenticate(string cnpj, string password) => conn.Query<Instituicao>("SELECT * FROM TB_INSTITUICAO WHERE sCNPJ = '82246853000172' AND sSenha='81DC9BDB52D04DC20036DBD8313ED055' ", new { cnpj, password = Diversos.GenerateMD5(password) }).FirstOrDefault();
        public Instituicao GetAuthenticate(string cnpj, string password) => conn.Query<Instituicao>("SELECT * FROM TB_INSTITUICAO WHERE sCNPJ = @cnpj AND sSenha=@sSenha ", new { cnpj, sSenha = Diversos.GenerateMD5(password) }).FirstOrDefault();

        public Auth GetHash(string hash) => conn.Query<Auth>("SELECT top(1) sCNPJ,sSenha = '',sConfirmaSenha = '' FROM TB_INSTITUICAO WHERE sHash =@hash ", new { hash }).FirstOrDefault();

        public int GetSend(string sCNPJ)
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                Instituicao _instituicao = conn.Query<Instituicao>("SELECT top(1) * FROM TB_INSTITUICAO WHERE sCNPJ =@sCNPJ ", new { sCNPJ }).FirstOrDefault();

                // generate a 128-bit salt using a secure PRNG
                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                string sHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: sCNPJ,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                string pattern = @"(?i)[^0-9a-záéíóúàèìòùâêîôûãõç\s]";
                string replacement = "";
                Regex rgx = new Regex(pattern);
                string result = rgx.Replace(sHash, replacement);

                conn.Execute("UPDATE TB_INSTITUICAO SET sHash=@sHash,dDataEnvioHash=GETDATE() WHERE sCNPJ = @sCNPJ ", new { sCNPJ, sHash = result });

                string _body = "";
                _body = "<tr bgcolor='#D6EFDA'><td colspan=2  style='font-weight:bold'>INSTITUIÇÃO</td></tr>";
                _body += "<tr style='=font-weight:bold;'>";
                _body += "<td font-weight:bold'><b>Proponente: </b>" + _instituicao.sProponente + "</td>";
                _body += "<td font-weight:bold'><b>CNPJ: </b>" + _instituicao.sCNPJ + "</td></tr>";

                _body += "<tr bgcolor='#D6EFDA'><td colspan=2  style='font-weight:bold'>Não consegue lembrar sua senha? Não tem problema, acontece com todos nós. Para criar uma nova senha, clique no link abaixo:</td></tr>";
                _body += config.GetSection(key: "Config")["sHostChangeSenha"] + result;

                string strBody = "";
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
                strBody = strBody + "Esta é uma  mensagem automática enviada pelo sistema. Não precisa responder.";
                strBody = strBody + "</body>";
                strBody = strBody + "</html>";

                Diversos.SendEmail(_instituicao.sEmail, "Mudança de senha FDID", strBody, null, null);

                return 1;
            }
            catch (Exception)
            {

                return 0;
            }

        }

        public int GetChangerPassword(string sCNPJ, string password)
        {
            try
            {
                DateTime dDataEnvioHash = conn.Query<DateTime>("SELECT top(1) dDataEnvioHash FROM TB_INSTITUICAO WHERE sCNPJ =@sCNPJ ", new { sCNPJ }).FirstOrDefault();

                if (dDataEnvioHash <= DateTime.Now.AddDays(1))
                {
                    conn.Execute("UPDATE TB_INSTITUICAO SET sSenha=@sSenha,sHash = null,dDataEnvioHash = null WHERE sCNPJ = @sCNPJ ", new { sCNPJ, sSenha = Diversos.GenerateMD5(password) });
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception)
            {

                return 2;
            }

        }

        public override IEnumerable<Instituicao> GetAll()  => conn.Query<Instituicao>("SELECT * FROM TB_INSTITUICAO").ToList();

        public bool GetValorContra(int id) => conn.Query<bool>("SELECT count(*) FROM TB_INSTITUICAO WHERE iNatureza = 1 AND ID=@id", new { id }).FirstOrDefault();
        public bool GetTestCnpj(string cnpj) => conn.Query<bool>("SELECT count(*) FROM TB_INSTITUICAO WHERE sCNPJ=@cnpj", new { cnpj }).FirstOrDefault();


        public void SendEmail(int IDInstituicao)
        {
            string _body = "";
            string strBody = "";

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Instituicao _instituicao = GetById(IDInstituicao);

            _body = "<tr bgcolor='#D6EFDA'><td colspan=2  style='font-weight:bold'>INSTITUIÇÃO</td></tr>";
            _body += "<tr style='=font-weight:bold;'>";
            _body += "<td font-weight:bold'><b>Proponente: </b>" + _instituicao.sProponente + "</td>";
            _body += "<td font-weight:bold'><b>CNPJ: </b>" + _instituicao.sCNPJ + "</td></tr>";

            _body += "<tr style='=font-weight:bold;'>";
            _body += "<td font-weight:bold'><b>Endereço: </b>" + _instituicao.sEndereco + "</td>";
            _body += "<td font-weight:bold'><b>CEP: </b>" + _instituicao.sCep + "</td></tr>";

            _body += "<tr style='=font-weight:bold;'>";
            _body += "<td font-weight:bold'><b>Telefone: </b>" + _instituicao.sTelefone + "</td>";
            _body += "<td font-weight:bold'<b>Fax: </b>" + _instituicao.sFax + "</td></tr>";

            _body += "<tr style='=font-weight:bold;'>";
            _body += "<td font-weight:bold' colspan=2><b>Municipio: </b>" + _instituicao.IDCidade + "</td></tr>";

            _body += "<tr style='=font-weight:bold;'>";
            _body += "<td font-weight:bold'<b>Email: </b>" + _instituicao.sEmail + "</td>";
            _body += "<td font-weight:bold'><b>HomePage: </b>" + _instituicao.sHomePage + "</td></tr>";

            _body += "<tr style='=font-weight:bold;'>";

            switch (_instituicao.iRegime)
            {
                case 1:
                    _body += "<td font-weight:bold'<b>Regime: </b>Direito Público</td>";

                    break;
                case 2:
                    _body += "<td font-weight:bold'<b>Regime: </b>Direito Privado</td>";

                    break;
            }

            switch (_instituicao.iEsfera)
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

            _body += "<td font-weight:bold'><b>Representante: </b>" + _instituicao.sRepresentante + "</td>";
            _body += "<td font-weight:bold'><b>Cpf: </b>" + _instituicao.sCpfRepresentante + "</td>";
            _body += "<tr><td font-weight:bold'><b>Cargo: </b>" + _instituicao.sCargo + "</td>";
            _body += "<td font-weight:bold'><b>Função: </b>" + _instituicao.sFuncao + "</td></tr>";
            _body += "<tr><td font-weight:bold'><b>Rg: </b>" + _instituicao.sRG + "</td>";
            _body += "<td font-weight:bold'><b>Órgão Expedidor: </b>" + _instituicao.sOrgaoExpedidor + "</td></tr>";
            _body += "<tr><td font-weight:bold' colspan=2><b>Endereço Representante: </b>" + _instituicao.sEnderecoRepresentante + "</td></tr>";
            _body += "<tr><td font-weight:bold'><b>Telefone: </b>" + _instituicao.sTelefoneRepresentante + "</td>";
            _body += "<td font-weight:bold'><b>CEP: </b>" + _instituicao.sCepRepresentante + "</td></tr>";

            _body += "<tr bgcolor='#D6EFDA'><td colspan=2  style='font-weight:bold'>COORDENADOR</td></tr>";
            _body += "<tr><td font-weight:bold'><b>Coordenador: </b>" + _instituicao.sCoordenador + "</td>";
            _body += "<td font-weight:bold'><b>Cpf: </b>" + _instituicao.sCPFCoordenador + "</td></tr>";
            _body += "<tr><td font-weight:bold'><b>Telefone: </b>" + _instituicao.sTelefoneCoordenador + "</TD>";
            _body += "<td font-weight:bold'><b>Fax: </b>" + _instituicao.sFaxCoordenador + "</td></tr>";

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
            strBody = strBody + "ATENÇÃO: O cadastro da instituição somente estará completo após a anexação dos arquivos relacionados à documentação comprobatória exigida em edital! ";
            strBody = strBody + "<a href='" + config.GetSection(key: "Config")["sSite"] + "'>Clique AQUI<a>";
            strBody = strBody + " para entrar no sistema e concluir o cadastro anexando os arquivos necessários.";
            strBody = strBody + "<br><br>";
            strBody = strBody + "Esta é uma  mensagem automática enviada pelo sistema. Não precisa responder.";
            strBody = strBody + "</body>";
            strBody = strBody + "</html>";

            Diversos.SendEmail(config.GetSection(key: "Config")["sEmailSend"], "[PROJETOS FDID] Nova instituição cadastrada", strBody, null, _instituicao.sEmail);
        }


    }
}
