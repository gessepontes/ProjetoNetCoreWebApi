using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces;

namespace mp.ce.fdid.FdidProjetos.Controllers
{
    [Produces("application/json")]
    [Route("api/Estado")]
    public class EstadoController : Controller
    {
        private readonly IEstadoRepository _estadoRepository;

        public EstadoController(IEstadoRepository estadoRepository)
        {
            _estadoRepository = estadoRepository;
        }

        // GET: api/Estado
        [HttpGet]
        public IEnumerable<Estado> Get()
        {
            return _estadoRepository.GetAll();
        }

        // GET: api/Cidade/5
        [HttpGet("{idCidade}")]
        public int GetIdEstato(int idCidade)
        {
            return _estadoRepository.GetIdEstato(idCidade);
        }
    }
}