using Microsoft.AspNetCore.Mvc;
using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces;
using System.Collections.Generic;

namespace mp.ce.fdid.FdidProjetos.Controllers
{
    [Produces("application/json")]
    [Route("api/Cidade")]
    public class CidadeController : Controller
    {
        private readonly ICidadeRepository _cidadeRepository;

        public CidadeController(ICidadeRepository cidadeRepository)
        {
            _cidadeRepository = cidadeRepository;
        }

        // GET: api/Cidade
        [HttpGet]
        public IEnumerable<Cidade> Get()
        {
            return _cidadeRepository.GetAll();
        }

        // GET: api/Cidade/5
        [HttpGet("{id}", Name = "GetByIdEstado")]
        public IEnumerable<Cidade> Get(int id)
        {
            return _cidadeRepository.GetByIdEstado(id);
        }

    }
}