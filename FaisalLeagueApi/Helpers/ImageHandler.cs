using FaisalLeague.Api.Common.Exceptions;
using FaisalLeague.ImageWriter.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaisalLeagueApi.Helpers
{
    public class ImageHandler : IImageHandler
    {
        private readonly IImageWriter _imageWriter;
        public ImageHandler(IImageWriter imageWriter)
        {
            _imageWriter = imageWriter;
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            var result = await _imageWriter.UploadImage(file);
            if (result == "invalid")
                throw new BadRequestException("Invalid image !");
            //return new ObjectResult(result);
            return result;
        }
    }
}
