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

        public override void Add(Instituicao obj)
        {
            string sql = "";

            sql = "INSERT INTO TB_INSTITUICAO VALUES (@sProponente,@sCNPJ,@sEndereco,@sCep,@sTelefone,@sFax,@IDCidade,@sEmail,@sHomePage,@iRegime,@iEsfera,@sRepresentante,@sCpfRepresentante," +
                "@sCargo,@sFuncao,@sRG,@sOrgaoExpedidor,@sEnderecoRepresentante,@sTelefoneRepresentante,@sCepRepresentante,@sCoordenador," +
                "@sCPFCoordenador,@sTelefoneCoordenador,@sFaxCoordenador,getdate(),@sSenha,getdate(),'',null)";

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
                obj.ID
            });
        }


        public override void Update(Instituicao obj)
        {
            string sql = "";
            string parametros = "";

            if (obj.sSenha != "") parametros = ",sSenha=@sSenha";

            sql = "UPDATE TB_INSTITUICAO SET sProponente=@sProponente,sCNPJ=@sCNPJ,sEndereco=@sEndereco,sCep=@sCep,sTelefone=@sTelefone,sFax=@sFax,IDCidade=@IDCidade,sEmail=@sEmail,sHomePage=@sHomePage,iRegime=@iRegime,iEsfera=@iEsfera,sRepresentante=@sRepresentante,sCpfRepresentante=@sCpfRepresentante," +
                "sCargo=@sCargo,sFuncao=@sFuncao,sRG=@sRG,sOrgaoExpedidor=@sOrgaoExpedidor,sEnderecoRepresentante=@sEnderecoRepresentante,sTelefoneRepresentante=@sTelefoneRepresentante,sCepRepresentante=@sCepRepresentante,sCoordenador=@sCoordenador," +
                "sCPFCoordenador=@sCPFCoordenador,sTelefoneCoordenador=@sTelefoneCoordenador,sFaxCoordenador=@sFaxCoordenador,dDataMovimentacao=getdate() " + parametros + " WHERE ID = @ID; ";

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
                obj.ID
            });
        }

        public override Instituicao GetById(int? id)
        {

            var instituicaoDictionary = new Dictionary<int, Instituicao>();

            var list = conn.Query<Instituicao, ArquivoInstituicao, Instituicao>(
                @"SELECT C.*,CI.iCodEstado IDEstado,T.* FROM TB_INSTITUICAO C INNER JOIN TB_CIDADE CI ON CI.iCodCidade = C.IDCidade  LEFT JOIN TB_ARQUIVOINSTITUICAO T ON C.ID = T.IDInstituicao WHERE C.ID= @id",
                map: (instituicao, arquivoInstituicao) =>
                {
                    Instituicao instituicaoEntry;

                    if (!instituicaoDictionary.TryGetValue(instituicao.ID, out instituicaoEntry))
                    {
                        instituicaoEntry = instituicao;
                        instituicaoEntry.ArquivoInstituicao = new List<ArquivoInstituicao>();
                        instituicaoDictionary.Add(instituicaoEntry.ID, instituicaoEntry);
                    }

                    instituicaoEntry.ArquivoInstituicao.Add(arquivoInstituicao);
                    return instituicaoEntry;
                },
                splitOn: "ID",
                param: new { id }).Distinct().FirstOrDefault();

            return list;
        }


        public Instituicao GetByIdInstituicao(int IdInstituicao)
        {
            Instituicao i = GetById(IdInstituicao);
            i.ArquivoInstituicao = conn.Query<ArquivoInstituicao>("SELECT * FROM TB_ARQUIVOINSTITUICAO WHERE IDInstituicao =@IDInstituicao ", new { IDInstituicao = IdInstituicao }).ToList();
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

                Diversos.SendEmail(_instituicao.sEmail, "Mudança de senha FDID", strBody, null);

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
    }
}
