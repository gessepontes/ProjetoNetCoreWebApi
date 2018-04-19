using Microsoft.AspNetCore.Mvc;
using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces;

namespace mp.ce.fdid.FdidProjetos.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private readonly IInstituicaoRepository _instituicaoRepository;
        private readonly IPrazoRepository _prazoRepository;

        public AuthController(IInstituicaoRepository instituicaoRepository, IPrazoRepository prazoRepository)
        {
            _instituicaoRepository = instituicaoRepository;
            _prazoRepository = prazoRepository;
        }

        [HttpGet]
        public bool Prazo() => _prazoRepository.Prazo();

        [HttpGet("{cnpj}/{password}")]
        public Instituicao GetAuthenticate(string cnpj, string password)
        {
            if (string.IsNullOrEmpty(cnpj) || string.IsNullOrEmpty(password))
                return null;

            var _instituicao = _instituicaoRepository.GetAuthenticate(cnpj, password);

            // check if username exists
            if (_instituicao == null)
                return null;

            // authentication successful
            return _instituicao;
        }

        [HttpGet("cnpj/{cnpj}", Name = "GetSend")]
        public int GetSend(string cnpj)
        {
            return _instituicaoRepository.GetSend(cnpj);
        }

        [HttpGet("hash/{hash}", Name = "GetHash")]
        public Auth GetHash(string hash)
        {
            return _instituicaoRepository.GetHash(hash);
        }

        public int GetChangerPassword(string cnpj, string password)
        {
            return _instituicaoRepository.GetChangerPassword(cnpj, password);
        }


    }
}