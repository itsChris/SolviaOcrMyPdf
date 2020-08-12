using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using SolviaOcrMyPdf.Models;
using Tesseract;

namespace SolviaOcrMyPdf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcrMyPdfController : ControllerBase
    {
        public const string folderName = "images/";
        public const string outPath = @"c:\solvia\PdfOut";
        public const string trainedDataFolderName = "tessdata";

        [HttpPost]  
        public FileStreamResult GetOcredPdf([FromForm] OcrModel request)
        {
            string tessPath = Path.Combine(trainedDataFolderName, "");
            string name = request.Image.FileName;
            Guid guid = Guid.NewGuid();

            string outFile = @$"c:\solvia\{Guid.NewGuid()}";

            using (IResultRenderer renderer = ResultRenderer.CreatePdfRenderer(outFile, tessPath))
            {
                using (renderer.BeginDocument("TitleOfDocument"))
                {
                    using (var engine = new TesseractEngine(tessPath, request.DestinationLanguage, EngineMode.Default))
                    {
                        using (var img = Pix.LoadFromFile(folderName + name))
                        {
                            using (var page = engine.Process(img, "InputName"))
                            {
                                renderer.AddPage(page);
                            }
                        }
                    }
                }
            }
            // outFile => CreatePdfRenderer adds .pdf to the filename .. 
            FileStream fs = new FileStream($"{outFile}.pdf", FileMode.Open, FileAccess.Read);
            return File(fs, "application/octet-stream");
        }
    }
}
