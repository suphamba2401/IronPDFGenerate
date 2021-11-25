namespace Trail.Application.Services.Pdf
{
    /// <summary>
    /// Options that can be supplied when generating a PDF using <see cref="PdfGen.TryGeneratePdf"/>.
    /// </summary>
    public class PdfGenOptions
    {
        /// <summary>
        /// The margins of the page, which defaults to <see cref="PageMargins.Default"/>. You can use <see cref="PageMargins.Zero"/> to remove them.
        /// </summary>
        public PageMargins PageMargins { get; set; } = PageMargins.Default;

        /// <summary>
        /// Enables or disables the footer.
        /// </summary>
        public bool EnableFooters { get; set; } = false;

        /// <summary>
        /// The title of the document displayed in the document footer.
        /// </summary>
        public string FooterTitle { get; set; } = null;

        /// <summary>
        /// The name of a client displayed in the document footer.
        /// </summary>
        public string FooterName { get; set; } = null;
        /// <summary>
        /// Enables or disables the page number.
        /// </summary>
        public PageFooterType FooterType { get; set; } = PageFooterType.PageNumber;

        public PdfPaperOrientation PaperOrientation { get; set; } = PdfPaperOrientation.Portrait;

        public enum PageFooterType
        {
            TitlePage,
            PageNumber,
            DynamicPageNumber,
            KAN
        }

        public enum PdfPaperOrientation
        {
            Portrait = 0,
            Landscape = 1
        }

    }
}

