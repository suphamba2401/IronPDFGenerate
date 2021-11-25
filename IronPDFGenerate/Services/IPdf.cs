using System.Collections.Generic;
using System.IO;

namespace Trail.Application.Services.Pdf
{
    /// <summary>
    /// Interface for manipulating and reading PDF documents. Different
    /// implementations of this interface may have slight visual differences.
    /// </summary>
    public interface IPdf
    {
        /// <summary>
        /// The raw data of this PDF represented as a memory stream.
        /// </summary>
        public MemoryStream Stream { get; }

        /// <summary>
        /// The number of pages this PDF has.
        /// </summary>
        public int PageCount { get; }

        /// <summary>
        /// Append a PDF to the back of this one.
        /// </summary>
        /// <param name="pdf">The PDF to append.</param>
        public void Append(IPdf pdf);

        /// <summary>
        /// Prepend a PDF to the front of this one.
        /// </summary>
        /// <param name="pdf">The PDF to prepend.</param>
        public void Prepend(IPdf pdf);
        
        /// <summary>
        /// Insert a PDF at a specifc page index, pushing everything else down by the
        /// length of the inserted PDF.
        /// </summary>
        /// <param name="index">The index to insert the PDF at.</param>
        /// <param name="pdf">The PDF to insert.</param>
        /// <returns>The number of pages inserted.</returns>
        public int Insert(int index, IPdf pdf);

        /// <summary>
        /// Removes a page from the pdf.
        /// </summary>
        /// <param name="index">The index to remove the page at.</param>
        public void Remove(int index);
        /// <summary>
        /// Extract headings from this PDF. Reads heading marker tokens defined in <see cref="PdfRead"/>.
        /// </summary>
        /// <returns>A list of headings, their heading levels and their locations.</returns>
        public IList<Heading> ExtractHeadings();

        /// <summary>
        /// Extracts all text from this PDF, including from headers and footers.
        /// </summary>
        /// <returns>The document as plain text.</returns>
        public string ExtractText();

        /// <summary>
        /// Returns the size of the specified page.
        /// </summary>
        /// <returns>The document as plain text.</returns>
        public System.Drawing.Size GetPageSize(int pageIndex);

        /// <summary>
        /// Inserts Html onto the existing pdf
        /// </summary>
        /// <param name="stamp">the html content to insert</param>
        ///  /// <param name="pageIndex">the 0 based index of the page to insert this to</param>
        public void StampPage(IronPdf.Editing.HtmlStamp stamp, int pageIndex);

        /// <summary>
        /// Returns the string content of a page
        /// </summary>
        /// <param name="pageIndex">the index of the page to scan</param>
        /// <returns></returns>
        public string GetPageContent(int pageIndex);

        public int ExtractPageNumber(string marker);

        /// <summary>
        /// Saves this PDF to disk.
        /// </summary>
        /// <param name="fileName">The file name or path.</param>
        public void SaveAs(string fileName);

        /// <summary>
        /// Rotate a single page 90 degrees counter clockwise
        /// </summary>
        /// <param name="pageIndex"></param>
        public void RotatePage(int pageIndex);

        /// <summary>
        /// Check if the pdf page orientation is portrait
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public bool IsPortrait(int pageIndex);
    }
}
