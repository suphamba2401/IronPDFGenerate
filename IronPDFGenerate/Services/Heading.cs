namespace Trail.Application.Services.Pdf
{
    /// <summary>
    /// Used to mark the location of a heading within a PDF.
    /// </summary>
    public class Heading
    {
        /// <summary>
        /// The text of the heading.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The level of the heading starting from 0, i.e. H1, H2, H5.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// The page the heading is on starting from 0.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// The starting index of the heading if all text has extracted from the PDF.
        /// </summary>
        public int Index { get; set; }
    }
}
