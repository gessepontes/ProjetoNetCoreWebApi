using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces;

namespace mp.ce.fdid.FdidProjetos.Controllers
{
    [Produces("application/json")]
    [Route("api/Projeto")]
    public class ProjetoController : Controller
    {
        private readonly IProjetoRepository _projetoRepository;

        public ProjetoController(IProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        // GET: api/Projeto
        public IEnumerable<Projeto> Get()
        {
            return _projetoRepository.GetAll();
        }

        // GET: api/Projeto/5
        [HttpGet("{id}", Name = "GetProjetoID")]
        [Authorize]
        public Projeto Get(int id)
        {
            return _projetoRepository.GetById(id);
        }

        [HttpGet("GetByIdIsntituicao/{id}", Name = "GetByIdIsntituicao")]
        [Authorize]
        public IEnumerable<Projeto> GetByIdIsntituicao(int id)
        {
            return _projetoRepository.GetByIdIsntituicao(id);
        }

        // POST: api/Projeto
        [HttpPost]
        [Authorize]
        public int Post([FromBody]Projeto _projeto)
        {
            try
            {
                int IDProjeto = _projetoRepository.AddProjeto(_projeto);

                if (IDProjeto != 0)
                {
                    _projetoRepository.SendEmail(IDProjeto, 1);
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        // PUT: api/Projeto/5
        [HttpPut]
        [Authorize]
        public int Put([FromBody]Projeto _projeto)
        {
            try
            {
                if (_projetoRepository.UpdateProjeto(_projeto))
                {
                    _projetoRepository.SendEmail(_projeto.ID, 2);

                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        // DELETE: api/Projeto/5
        [HttpDelete("{id}")]
        [Authorize]
        public int Delete(int id)
        {
            try
            {
                var _projeto = _projetoRepository.GetById(id);

                if (_projetoRepository.RemoveProjeto(_projeto))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch (System.Exception)
            {
                return 0;
            }
        }

    }
}