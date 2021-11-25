using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IronPDFSample.Services;
using Trail.Application.Services.Pdf;

namespace IronPDFGenerate.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var reader = new StreamReader(Server.MapPath(Url.Content("~/Content/SOAHTML-185178.html")));
            var htmlString = reader.ReadToEnd();
            var options = new PdfGenOptions
            {
                FooterTitle = "Statement of Advice",
                FooterName = "Applicant Name",
                EnableFooters = true,
                FooterType = PdfGenOptions.PageFooterType.DynamicPageNumber
            };
            var pdf = PdfGen.GeneratePdf(htmlString, options);
            return File(pdf.Stream.ToArray(), $"application/pdf", "SOAHTML-185178.pdf");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
