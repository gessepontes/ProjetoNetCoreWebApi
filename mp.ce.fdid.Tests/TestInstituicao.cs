using Microsoft.VisualStudio.TestTools.UnitTesting;
using mp.ce.fdid.Data.Repositories;
using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.FdidProjetos.Controllers;
using System.Linq;

namespace mp.ce.fdid.Tests
{
    [TestClass]
    public class TestInstituicao
    {
        [TestMethod]
        public void TestGetAll()
        {
            bool test;

            var instituicaoRepository = new InstituicaoRepository();
            var controller = new InstituicaoController(instituicaoRepository);

            if (controller.Get() == null)
            {
                test = false;
            }
            else
            {
                test = true;
            }

            Assert.IsTrue(test);
        }

        [TestMethod]
        public void TestGet()
        {
            bool test;
            var instituicaoRepository = new InstituicaoRepository();
            var controller = new InstituicaoController(instituicaoRepository);

            var list = controller.Get();

            Instituicao i = list.Last();

            var result = controller.Get(i.ID);

            if (result == null)
            {
                test = false;
            }
            else {
                test = true;
            }

            Assert.IsTrue(test);
        }

        [TestMethod]
        public void TestPost()
        {
            bool test;

            var instituicaoRepository = new InstituicaoRepository();
            var controller = new InstituicaoController(instituicaoRepository);
            Instituicao instituicao = new Instituicao
            {
                IDCidade = 1,
                iEsfera = 1,
                iRegime = 1,
                sCargo = "qqqqqq",
                cPerfil = 'U',
                sCep = "60000000",
                sCepRepresentante = "60000000",
                sCNPJ = "1111122222233333",
                sCoordenador = "Test 1",
                sCPFCoordenador = "12345678910",
                sCpfRepresentante = "12345678910",
                sEmail = "teste@gmail.com",
                sEndereco = "rua a",
                sEnderecoRepresentante = "rua b",
                sFax = "85999999999",
                sFaxCoordenador = "85999999999",
                sFuncao = "Função",
                sHomePage = "http://www.site.com.br",
                sOrgaoExpedidor = "ssp ce",
                sProponente = "Proponente",
                sRepresentante = "Representante",
                sRG = "888888888888",
                sTelefone = "85999999999",
                sTelefoneCoordenador = "85999999999",
                sTelefoneRepresentante = "85999999999",
                sSenha = "123456"
            };

            if (controller.Post(instituicao) != 1)
            {
                test = false;
            }
            else
            {
                test = true;
            }

            Assert.IsTrue(test);
        }

        [TestMethod]
        public void TestPut()
        {
            bool test;

            var instituicaoRepository = new InstituicaoRepository();
            var controller = new InstituicaoController(instituicaoRepository);

            var list = controller.Get();

            Instituicao i = list.Last();

            i.sProponente = "Proponente";
            i.sRepresentante = "Representante";
            i.sRG = "888888888888";
            i.sTelefone = "85999999999";
            i.sTelefoneCoordenador = "85999999999";
            i.sTelefoneRepresentante = "85999999999";
            i.sSenha = "123456";

            if (controller.Put(i) != 1)
            {
                test = false;
            }
            else
            {
                test = true;
            }

            Assert.IsTrue(test);
        }

        [TestMethod]
        public void TestDelete()
        {
            bool test;

            var instituicaoRepository = new InstituicaoRepository();
            var controller = new InstituicaoController(instituicaoRepository);

            var list = controller.Get();

            Instituicao i = list.Last();


            if (controller.Delete(i.ID) != 1)
            {
                test = false;
            }
            else
            {
                test = true;
            }

            Assert.IsTrue(test);
        }
    }
}
