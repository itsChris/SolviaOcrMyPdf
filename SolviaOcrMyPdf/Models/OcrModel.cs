using Microsoft.AspNetCore.Http;
using System;

namespace SolviaOcrMyPdf.Models
{
    public class OcrModel
    {
        public string DestinationLanguage { get; set; }
        public IFormFile Image { get; set; }
    }
}
