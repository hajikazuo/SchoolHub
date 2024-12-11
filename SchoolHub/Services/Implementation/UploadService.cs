using ClosedXML.Excel;
using SchoolHub.Common.Models.Enums;
using SchoolHub.Common.Models.Usuarios;
using SchoolHub.Mvc.Services.Interface;

namespace SchoolHub.Mvc.Services.Implementation
{
    public class uploadService : IUploadService
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public uploadService(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public async Task<string> UploadFoto(IFormFile file, PastaUpload pastaUpload)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            string folderPath = pastaUpload switch
            {
                PastaUpload.FotoUsuario => Path.Combine(_env.WebRootPath, _config.GetValue<string>("FolderUpload:Usuarios")),
                PastaUpload.LogoTennant => Path.Combine(_env.WebRootPath, _config.GetValue<string>("FolderUpload:Logos")),
                _ => throw new ArgumentException("Tipo de pasta inválido.", nameof(pastaUpload))
            };

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public async Task<List<Usuario>> ProcessarExcel(IFormFile arquivoExcel)
        {
            if (arquivoExcel == null || arquivoExcel.Length == 0)
            {
                return null;
            }

            if (!ValidarExtensaoArquivoExcel(arquivoExcel))
            {
                return null;
            }

            var usuarios = new List<Usuario>();

            using (var stream = arquivoExcel.OpenReadStream())
            {
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheets.Worksheet(1);
                    var rowCount = worksheet.RowsUsed().Count();

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var nome = worksheet.Cell(row, 1).GetValue<string>();
                        var email = worksheet.Cell(row, 2).GetValue<string>();
                        var celular = worksheet.Cell(row, 3).GetValue<string>();

                        if (!string.IsNullOrWhiteSpace(nome) && !string.IsNullOrWhiteSpace(email))
                        {
                            usuarios.Add(new Usuario
                            {
                                Nome = nome,
                                Email = email,
                                UserName = email,
                                Celular = celular,
                                TennantId = null
                            });
                        }
                    }
                }
            }

            return usuarios;
        }

        private bool ValidarExtensaoArquivoExcel(IFormFile arquivoExcel)
        {
            var extensao = Path.GetExtension(arquivoExcel.FileName).ToLower();
            var extensoesValidas = new List<string> { ".xlsx", ".xls" };

            return extensoesValidas.Contains(extensao);
        }

    }
}
