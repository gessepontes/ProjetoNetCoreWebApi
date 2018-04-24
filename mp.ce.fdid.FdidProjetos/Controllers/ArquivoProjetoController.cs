using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mp.ce.fdid.Domain.Diversos;
using mp.ce.fdid.Domain.Entities;
using mp.ce.fdid.Domain.Interfaces;

namespace mp.ce.fdid.FdidProjetos.Controllers
{
    [Produces("application/json")]
    [Route("api/ArquivoProjeto")]
    public class ArquivoProjetoController : Controller
    {
        private readonly IArquivoProjetoRepository _arquivoProjetoRepository;

        public ArquivoProjetoController(IArquivoProjetoRepository arquivoProjetoRepository)
        {
            _arquivoProjetoRepository = arquivoProjetoRepository;
        }

        [HttpPost]
        public async Task<int> UploadAsync(IFormFile file, int IDProjeto)
        {
            if (file == null) throw new Exception("File is null");
            if (file.Length == 0) throw new Exception("File is empty");

            try
            {
                using (Stream stream = file.OpenReadStream())
                {
                    using (var binaryReader = new BinaryReader(stream))
                    {
                        var fileContent = binaryReader.ReadBytes((int)file.Length);

                        string[] aFoto = file.FileName.Split('.');

                        ArquivoProjeto _arquivoProjeto = new ArquivoProjeto();

                        _arquivoProjeto.sNome = IDProjeto + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + aFoto[aFoto.Count() - 1];
                        _arquivoProjeto.IDProjeto = IDProjeto;
                        _arquivoProjetoRepository.Add(_arquivoProjeto);
                        await Diversos.SaveImage(file, "PROJETO", _arquivoProjeto.sNome);
                    }
                }

                return 1;
            }
            catch (Exception)
            {

                return 0;
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Download(int id)
        {
            try
            {
                var _arquivoProjeto = _arquivoProjetoRepository.GetById(id);

                if (_arquivoProjeto.sNome == null)
                    return Content("Arquivo não encontrado");

                string path = Diversos.PathArquivo(_arquivoProjeto.sNome, "PROJETO");

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, Diversos.GetContentType(path), Path.GetFileName(path));
            }
            catch (Exception)
            {

                throw;
            }
        }

         // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize]
        public int Delete(int id)
        {
            try
            {
                var _arquivoProjeto = _arquivoProjetoRepository.GetById(id);
                _arquivoProjetoRepository.Remove(_arquivoProjeto);

                string path = Diversos.PathArquivo(_arquivoProjeto.sNome, "PROJETO");

                System.IO.File.Delete(path);

                return 1;
            }
            catch (System.Exception)
            {
                return 0;
            }
        }
    }
}