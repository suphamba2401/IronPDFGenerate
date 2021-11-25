using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using IronPdf;

namespace Trail.Application.Services.Pdf
{
    /// <summary>
    /// IronPDF backed implementation of the <see cref="IPdf"/> interface.
    /// </summary>
    public class IronPdfDoc : IPdf
    {
        public MemoryStream Stream => internalPdf.Stream;
        public int PageCount => internalPdf.PageCount;

        private PdfDocument internalPdf;

        public IronPdfDoc(PdfDocument pdf)
        {
            internalPdf = pdf;
        }

        /// <summary>
        /// Attempts to generate an IronPdf document from a byte array, this performs known checks for pdfs that do not work with IronPdf
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="ironPdf"></param>
        /// <returns></returns>
        public static bool TryGetPdfDocument(byte[] bytes, bool removeEncryption, out IronPdfDoc ironPdf)
        {
            ironPdf = null;

            try
            {
                // try to generate a pdf file
                var pdf = new PdfDocument(bytes);

                if (removeEncryption)
                {
                    pdf.SecuritySettings.RemovePasswordsAndEncryption();
                }

                // Check the known signs of a pdf that will be corrupted by IronPdf
                // Checking pdf.BinaryData with null here resulted in a lot of extra CPU usage. pdf.Stream.Length improved perforance by 25%.
                if (pdf.PageCount == 0 || pdf.Stream.Length <= 0 || (pdf.MetaData.Producer.Contains("Microsoft") && !pdf.MetaData.Producer.Contains("iTextSharp")))                
                {
                    return false;
                }

                ironPdf = new IronPdfDoc(pdf);
            }
            catch 
            {
                return false;
            }

            return true;
        }

        public void Append(IPdf pdf)
        {
            if (pdf is IronPdfDoc ipd)
                internalPdf.AppendPdf(ipd.internalPdf);
            else
                internalPdf.AppendPdf(new PdfDocument(pdf.Stream));
        }

        public void Prepend(IPdf pdf)
        {
            if (pdf is IronPdfDoc ipd)
                internalPdf.PrependPdf(ipd.internalPdf);
            else
                internalPdf.PrependPdf(new PdfDocument(pdf.Stream));
        }

        public int Insert(int index, IPdf pdf)
        {
            var insertedPages = pdf.PageCount;
            var pdfDocument = new PdfDocument(pdf.Stream);
            if (pdf is IronPdfDoc ipd)
                pdfDocument = new PdfDocument(ipd.Stream);

            for (int i = 0; i < insertedPages; i++)
            {
                PdfDocument outputDocument = pdfDocument.CopyPage(i);
                internalPdf.InsertPdf(outputDocument, index++);
            }
            return insertedPages;
        }

        public void Remove(int index)
        {
            if (index == -1 || index >= internalPdf.PageCount)
            {
                return;
            }
            internalPdf.RemovePage(index);
        }

        public System.Drawing.Size GetPageSize(int pageIndex)
        {
            return new System.Drawing.Size { Width = (int)internalPdf.Pages[pageIndex].PrintWidth, Height = (int)internalPdf.Pages[pageIndex].PrintHeight };
        }

        public void StampPage(IronPdf.Editing.HtmlStamp stamp, int pageIndex)
        {
            internalPdf.StampHTML(stamp, pageIndex);
        }

        public IList<Heading> ExtractHeadings()
        {
            var headings = new List<Heading>();

            foreach (var pageIndex in Enumerable.Range(0, internalPdf.PageCount))
            {
                var text = internalPdf.ExtractTextFromPage(pageIndex)
                    .Replace("\r", "")
                    .Replace("\n", "");

                // TODO use this to get a series of special charaters, and their html equivalents 
                // https://dev.w3.org/html5/html-author/charref
                // figure out the purpose of the base css/cshtml pages and sort that out


                // Use this regex to extract headers (?<=(SEARCH_TOKEN))(.+) (Goes to end of line)
                //var h1Matches = new Regex($@"(?<=({PdfRead.H1BeginToken}))(.*?)(?=({PdfRead.H1EndToken}))").Matches(text);
                //var h2Matches = new Regex($@"(?<=({PdfRead.H2BeginToken}))(.*?)(?=({PdfRead.H2EndToken}))").Matches(text);
                //var h3Matches = new Regex($@"(?<=({PdfRead.H3BeginToken}))(.*?)(?=({PdfRead.H3EndToken}))").Matches(text);
                //var h4Matches = new Regex($@"(?<=({PdfRead.H4BeginToken}))(.*?)(?=({PdfRead.H4EndToken}))").Matches(text);
                //var h5Matches = new Regex($@"(?<=({PdfRead.H5BeginToken}))(.*?)(?=({PdfRead.H5EndToken}))").Matches(text);
                //var h6Matches = new Regex($@"(?<=({PdfRead.H6BeginToken}))(.*?)(?=({PdfRead.H6EndToken}))").Matches(text);

                //headings.AddRange(PdfRead.ExtractHeadingsFromMatches(h1Matches, 0, pageIndex));
                //headings.AddRange(PdfRead.ExtractHeadingsFromMatches(h2Matches, 1, pageIndex));
                //headings.AddRange(PdfRead.ExtractHeadingsFromMatches(h3Matches, 2, pageIndex));
                //headings.AddRange(PdfRead.ExtractHeadingsFromMatches(h4Matches, 3, pageIndex));
                //headings.AddRange(PdfRead.ExtractHeadingsFromMatches(h5Matches, 4, pageIndex));
                //headings.AddRange(PdfRead.ExtractHeadingsFromMatches(h6Matches, 5, pageIndex));
            }

            return headings
                .OrderBy(n => n.Page)
                .ThenBy(n => n.Index)
                .ToList();
        }

        public string ExtractText() =>
            internalPdf.ExtractAllText();

        public string GetPageContent(int pageIndex)
        {
            return internalPdf.ExtractTextFromPage(pageIndex);
        }

        public void SaveAs(string fileName) =>
            internalPdf.SaveAs(fileName);

        public int ExtractPageNumber(string marker)
        {
            foreach (var pageIndex in Enumerable.Range(0, internalPdf.PageCount))
            {
                var text = internalPdf.ExtractTextFromPage(pageIndex)
                    .Replace("\r", "")
                    .Replace("\n", "");             
                if (text.Contains(marker)) return pageIndex;
            }
            return -1;
        }

        public bool IsPortrait(int pageIndex)
        {
            var page = internalPdf.Pages[pageIndex];
            return page.PageOrientation == PageOrientation.Portrait;
        }

        /// <summary>
        /// Roates a page clockwise by 90 degrees
        /// </summary>
        /// <param name="pageIndex"></param>
        public void RotatePage(int pageIndex)
        {
            int desiredRot = 0; 
            var rotation = (internalPdf.GetPageRotation(pageIndex)) * -1;
            internalPdf.RotatePage(pageIndex, (desiredRot + rotation) % 360);
        }
    }
}
