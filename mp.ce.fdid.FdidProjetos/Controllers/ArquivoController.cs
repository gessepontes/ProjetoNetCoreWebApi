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
    [Route("api/Arquivo")]
    public class ArquivoController : Controller
    {
        private readonly IArquivoRepository _arquivoRepository;

        public ArquivoController(IArquivoRepository arquivoRepository)
        {
            _arquivoRepository = arquivoRepository;
        }

        [HttpPost]
        public async Task<int> UploadAsync(IFormFile file, int IDInstituicaoProjeto,int iTipo)
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
                        string sTipo;
                        string[] aFoto = file.FileName.Split('.');

                        Arquivo _arquivoProjeto = new Arquivo
                        {
                            sNome = aFoto[0],
                            sNomebase = IDInstituicaoProjeto + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + aFoto[aFoto.Count() - 1],
                            IDInstituicaoProjeto = IDInstituicaoProjeto,
                            iTipo = iTipo
                        };

                        _arquivoRepository.Add(_arquivoProjeto);

                        if (iTipo == 1) {
                            sTipo = "INSTITUICAO";
                        } else {
                            sTipo = "PROJETO";
                        }

                        await Diversos.SaveImage(file, sTipo, _arquivoProjeto.sNomebase);
                    }
                }

                return 1;
            }
            catch (Exception)
            {

                return 0;
            }

        }

        [HttpGet("{id}/{iTipo}")]
        public async Task<IActionResult> Download(int id, int iTipo)
        {
            try
            {
                var _arquivoProjeto = _arquivoRepository.GetById(id);
                string sTipo;

                if (_arquivoProjeto.sNome == null)
                    return Content("Arquivo não encontrado");

                if (iTipo == 1)
                {
                    sTipo = "INSTITUICAO";
                }
                else
                {
                    sTipo = "PROJETO";
                }

                string path = Diversos.PathArquivo(_arquivoProjeto.sNomebase, sTipo);

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
                var _arquivoProjeto = _arquivoRepository.GetById(id);
                _arquivoRepository.Remove(_arquivoProjeto);
                string sTipo;


                if (_arquivoProjeto.iTipo == 1)
                {
                    sTipo = "INSTITUICAO";
                }
                else
                {
                    sTipo = "PROJETO";
                }

                string path = Diversos.PathArquivo(_arquivoProjeto.sNomebase, sTipo);

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