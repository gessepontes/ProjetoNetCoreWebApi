using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

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


        [HttpGet("token/{user}")]
        public string GetBuildToken(string user)
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var claims = new[]
                {
                  new Claim(JwtRegisteredClaimNames.Sub, user),
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection(key: "Config")["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(config.GetSection(key: "Config")["Issuer"], config.GetSection(key: "Config")["Issuer"],
              expires: DateTime.Now.AddMinutes(Convert.ToInt16(config.GetSection(key: "Config")["JwtExpireMinutes"])),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}