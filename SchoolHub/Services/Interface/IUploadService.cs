using Microsoft.AspNetCore.Http;
using SchoolHub.Common.Models.Enums;
using SchoolHub.Common.Models.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Mvc.Services.Interface
{
    public interface IUploadService
    {
        Task<string> UploadFoto(IFormFile file, PastaUpload pastaUpload);

        Task<List<Usuario>> ProcessarExcel(IFormFile? arquivoExcel);
    }
}
