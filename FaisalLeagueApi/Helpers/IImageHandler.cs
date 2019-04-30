using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaisalLeagueApi.Helpers
{
    public interface IImageHandler
    {
        Task<string> UploadImage(IFormFile file);
    }
}
