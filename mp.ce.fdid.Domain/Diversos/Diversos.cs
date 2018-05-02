using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;

namespace mp.ce.fdid.Domain.Diversos
{
    public class Diversos
    {
        public static string GenerateMD5(string yourString)
        {
            if (yourString == "" || yourString == null)
            {
                yourString = "123456";
            }
            return string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(yourString)).Select(s => s.ToString("x2")));
        }

        public static async Task SaveImage(IFormFile foto, string destino, string sFoto)
        {
            string path = PathArquivo(sFoto, destino);

            if (foto == null || foto.Length == 0)
            {

            }
            else
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }

            }
        }

        public static string PathArquivo(string sFoto, string origem)
        {
            string path = "";

            switch (origem)
            {
                case "INSTITUICAO":
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\doc\\instituicao", sFoto);
                    break;
                case "PROJETO":
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\doc\\projeto", sFoto);
                    break;
            }


            return path;
        }

        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".odt", "application/vnd.oasis.opendocument.text"},
                {".ods", "application/vnd.oasis.opendocument.spreadsheet"},
                {".odp", "application/vnd.oasis.opendocument.presentation"}
            };
        }

        public static string ReadSetting(string key)
        {
            string result = "";

            try
            {
                result = ConfigurationManager.AppSettings[key].ToString();
            }
            catch (ConfigurationErrorsException)
            {
            }

            return result;

        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }
            else
            {
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
            }

        }


        public static bool SendEmail(string _to, string _subject, string _body, List<string> _anexos, string _cc)
        {
            try
            {
                AlternateView view = AlternateView.CreateAlternateViewFromString(_body, null, MediaTypeNames.Text.Html);
                LinkedResource resource = new LinkedResource(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\logot.png"));

                resource.ContentId = "Imagem1";
                view.LinkedResources.Add(resource);

                MailMessage mailMessage = new MailMessage("sistemas.setin@mpce.mp.br", _to, _subject, _body)
                {
                    IsBodyHtml = true
                };


                if (_cc != null)
                {
                    MailAddress cc = new MailAddress(_cc);
                    mailMessage.CC.Add(cc);
                }


                if (_anexos != null) {
                    foreach (string item in _anexos)
                    {
                        mailMessage.Attachments.Add(new Attachment(item));
                    }
                }

                mailMessage.AlternateViews.Add(view);
                mailMessage.Priority = MailPriority.Normal;

                SmtpClient client = new SmtpClient("pgjsrv129",25);
                client.UseDefaultCredentials = true;

                client.Send(mailMessage);

                return true;
            }
            catch (Exception e)
            {
                e.Message.ToString();
                return false;
            }

        }
    }
}
