using Microsoft.AspNetCore.Http;
using SchoolHub.Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolHub.Mvc.Services.Interface
{
    public interface IUploadFotoService
    {
        Task<string> UploadFoto(IFormFile file, PastaUpload pastaUpload);
    }
}
