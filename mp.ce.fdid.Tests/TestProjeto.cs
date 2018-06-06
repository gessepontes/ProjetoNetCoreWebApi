using Microsoft.VisualStudio.TestTools.UnitTesting;
using mp.ce.fdid.Data.Repositories;
using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.FdidProjetos.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mp.ce.fdid.Tests
{
    [TestClass]
    public class TestProjeto
    {

        [TestMethod]
        public void TestGetAll()
        {
            bool test;
            var projetoRepository = new ProjetoRepository();
            var controller = new ProjetoController(projetoRepository);

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
            var projetoRepository = new ProjetoRepository();
            var controller = new ProjetoController(projetoRepository);

            IEnumerable<Projeto> list = controller.Get();

            Projeto i = list.Last();

            var result = controller.Get(i.ID);

            if (result == null)
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
        public void TestPost()
        {
            bool test;

            var projetoRepository = new ProjetoRepository();
            var controller = new ProjetoController(projetoRepository);

            Projeto projeto = new Projeto
            {
                IDCidade = 1,
                IDInstituicao = 1,
                mValor = 1,
                mValorContraPartida = 1,
                sTitulo = "Titulo",
                sNoProcesso = "1111/1111",
                tResumo = "1111122222233333",
                dDataInicio = DateTime.Now.Date,
                dDataTermino = DateTime.Now.Date
            };

            if (controller.Post(projeto) != 1)
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
            var projetoRepository = new ProjetoRepository();
            var controller = new ProjetoController(projetoRepository);

            IEnumerable<Projeto> list = controller.Get();

            Projeto i = list.Last();

            i.IDCidade = 2;
            i.IDInstituicao = 1;
            i.mValor = 1;
            i.mValorContraPartida = 1;
            i.sTitulo = "Titulo";
            i.sNoProcesso = "111/1111";
            i.tResumo = "1111122222233333";

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

            var projetoRepository = new ProjetoRepository();
            var controller = new ProjetoController(projetoRepository);

            IEnumerable<Projeto> list = controller.Get();

            Projeto i = list.Last();

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
