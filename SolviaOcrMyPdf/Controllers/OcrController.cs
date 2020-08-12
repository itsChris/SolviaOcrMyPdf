using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using SolviaOcrMyPdf.Models;
using Tesseract;

namespace SolviaOcrMyPdf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcrController : ControllerBase
    {
        public const string folderName = "images/";
        public const string outPath = "out/";
        public const string trainedDataFolderName = "tessdata";

        [HttpPost]
        public string DoOCR([FromForm] OcrModel request)
        {
            string tessPath = Path.Combine(trainedDataFolderName, "");
            string name = request.Image.FileName;
            var image = request.Image;
            if (image.Length > 0)
            {
                using (var fileStream = new FileStream(folderName + image.FileName, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            string result = "";
            using (var engine = new TesseractEngine(tessPath, request.DestinationLanguage, EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(folderName + name))
                {
                    var page = engine.Process(img);
                    result = page.GetText();
                    Console.WriteLine(result);
                }
            }
            return string.IsNullOrWhiteSpace(result) ? "Ocr has completed. Return empty" : result;
        }
    }
}
