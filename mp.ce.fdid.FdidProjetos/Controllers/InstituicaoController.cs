using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces;
using System.Collections.Generic;

namespace mp.ce.fdid.FdidProjetos.Controllers
{
    [Produces("application/json")]
    [Route("api/Instituicao")]
    public class InstituicaoController : Controller
    {

        private readonly IInstituicaoRepository _instituicaoRepository;

        public InstituicaoController(IInstituicaoRepository instituicaoRepository)
        {
            _instituicaoRepository = instituicaoRepository;
        }


        // GET: api/Instituicao/5
        [HttpGet("{id}")]
        [Authorize]
        public Instituicao Get(int id)
        {
            return _instituicaoRepository.GetById(id);
        }

        [Authorize]
        public IEnumerable<Instituicao> Get()
        {
            return _instituicaoRepository.GetAll();
        }

        [HttpGet("GetValorContra/{id}", Name = "GetValorContra")]
        public bool GetValorContra(int id)
        {
            return _instituicaoRepository.GetValorContra(id);
        }

        // POST: api/Instituicao
        [HttpPost]
        public int Post([FromBody]Instituicao _instituicao)
        {
            try
            {
                _instituicaoRepository.Add(_instituicao);
                return 1;
            }
            catch (System.Exception e)
            {
                e.Message.ToString();
                return 0;
            }
        }

        // PUT: api/Instituicao
        [HttpPut]
        [Authorize]
        public int Put([FromBody]Instituicao _instituicao)
        {
            try
            {
                _instituicaoRepository.Update(_instituicao);
                return 1;
            }
            catch (System.Exception e)
            {
                e.Message.ToString();
                return 0;
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize]
        public int Delete(int id)
        {
            try
            {
                var _instituicao = _instituicaoRepository.GetById(id);
                _instituicaoRepository.RemoveInstituicao(_instituicao);
                return 1;
            }
            catch (System.Exception)
            {
                return 0;
            }
        }
    }
}
