using IronPdf;
using Trail.Application.Services.Pdf;

namespace IronPDFSample.Services
{
    public class PdfGen
    {
        /// <summary>
        /// Creates a PDF from a string of HTML.
        /// </summary>
        /// <param name="html">The HTML to generate a PDF from.</param>
        /// <param name="options">Options for stuff like margins and footers.</param>
        /// <returns></returns>
        public static IPdf GeneratePdf(string html, PdfGenOptions options = null)
        {
            var renderer = new IronPdf.ChromePdfRenderer();
            var htmlFragment = "";

            renderer.RenderingOptions.CssMediaType = IronPdf.Rendering.PdfCssMediaType.Screen;

            if (options != null)
            {
                renderer.RenderingOptions.MarginTop = options.PageMargins.Top;
                renderer.RenderingOptions.MarginBottom = options.PageMargins.Bottom;
                renderer.RenderingOptions.MarginLeft = options.PageMargins.Left;
                renderer.RenderingOptions.MarginRight = options.PageMargins.Right;
                renderer.RenderingOptions.PaperOrientation = (IronPdf.Rendering.PdfPaperOrientation)options.PaperOrientation;

                if (options.EnableFooters)
                {
                    htmlFragment = options.FooterType switch
                    {
                        PdfGenOptions.PageFooterType.TitlePage => PdfFooter.GenerateTitlePageFooter(options),
                        PdfGenOptions.PageFooterType.PageNumber => PdfFooter.GeneratePageNumberFooter(options),
                        PdfGenOptions.PageFooterType.DynamicPageNumber => PdfFooter.GenerateDynamicPageNumberFooter(options),
                        PdfGenOptions.PageFooterType.KAN => PdfFooter.GenerateKANFooter(options),
                        _ => ""
                    };

                    if (options.FooterType == PdfGenOptions.PageFooterType.TitlePage)
                    {
                        renderer.RenderingOptions.MarginBottom = 20;
                    }
                    else if (options.FooterType == PdfGenOptions.PageFooterType.PageNumber)
                    {
                        // JDW 2021/09/27 - Iron PDF Footer Bug
                        renderer.RenderingOptions.HtmlFooter = new HtmlHeaderFooter
                        {
                            Height = 15,
                            FontFamily = "Arial",
                            LoadStylesAndCSSFromMainHtmlDocument = true,
                            HtmlFragment = @"<div style='display: flex; flex-direction: row; justify-content: flex-end; box-sizing: border-box; width: calc(100% - 8em); font-size: 13px; margin-bottom: 20px; margin-left: 4em; margin-right: 4em; color: #9F9F9F; text-align: left; font-family: Arial;'>Page {page}/{total-pages}</div>",
                        };
                    }
                    else if (options.FooterType == PdfGenOptions.PageFooterType.DynamicPageNumber)
                    {
                        // JDW 2021/09/27 - Iron PDF Footer Bug
                        renderer.RenderingOptions.HtmlFooter = new HtmlHeaderFooter
                        {
                            Height = 15,
                            FontFamily = "Arial",
                            LoadStylesAndCSSFromMainHtmlDocument = true,
                            HtmlFragment = @$"<div style='display: flex; flex-direction: row; justify-content: flex-end; box-sizing: border-box; width: calc(100% - 8em); font-size: 13px; margin-bottom: 20px; margin-left: 4em; margin-right: 4em; color: #fff; text-align: left; font-family: Arial;'>{PdfRead.PageNumberToken}</div>",
                        };
                    }
                }
            }
            var pdf = renderer.RenderHtmlAsPdf(html); // "~/Views/PDFs/"

            var foregroundStamp = new IronPdf.Editing.HtmlStamp()
            {
                Html = htmlFragment,
                Width = 210,
                Height = 20,
                Opacity = 100,
                Bottom = 3,
                ZIndex = IronPdf.Editing.HtmlStamp.StampLayer.OnTopOfExistingPDFContent
            };
            pdf.StampHTMLAsync(foregroundStamp).Wait();

            // Use IronPDF as Trail's PDF implementation.
            return new IronPdfDoc(pdf);
        }
    }
}
