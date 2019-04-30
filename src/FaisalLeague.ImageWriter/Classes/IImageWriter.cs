using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FaisalLeague.ImageWriter.Classes
{
    public interface IImageWriter
    {
        Task<string> UploadImage(IFormFile file);
    }
}
