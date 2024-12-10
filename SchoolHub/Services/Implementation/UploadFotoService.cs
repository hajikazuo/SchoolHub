using SchoolHub.Common.Models.Enums;
using SchoolHub.Mvc.Services.Interface;

namespace SchoolHub.Mvc.Services.Implementation
{
    public class UploadFotoService : IUploadFotoService
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public UploadFotoService(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public async Task<string> UploadFoto(IFormFile file, PastaUpload pastaUpload)
        {
            if (file == null || file.Length == 0)
                return null;

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
    }
}
