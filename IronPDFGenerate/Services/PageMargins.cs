namespace Trail.Application.Services.Pdf
{
    /// <summary>
    /// Defines margins of a page.
    /// </summary>
    public struct PageMargins
    {
        public int Top, Bottom, Left, Right;

        /// <summary>
        /// Default page margins.
        /// </summary>
        public static PageMargins Default =>
            new PageMargins
            {
                Top = 16,
                Bottom = 20,
                Left = 16,
                Right = 16
            };

        /// <summary>
        /// No page margins.
        /// </summary>
        public static PageMargins Zero => 
            new PageMargins();
    }
}

