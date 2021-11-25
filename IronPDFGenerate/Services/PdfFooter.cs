namespace Trail.Application.Services.Pdf
{
    /// <summary>
    /// Utilities for rendering Razor cshtml templates and turning them into PDFs.
    /// </summary>
    public static class PdfFooter
    {
        public static string GenerateTitlePageFooter(PdfGenOptions options)
        {
            return $@"<hr color='#D4E1F0' style='margin-left: 3.0em; margin-right: 3.0em;'>
                <div style='display: flex; 
                    flex-direction: row;
                    justify-content: space-between;
                    box-sizing: border-box;
                    width: calc(100% - 8em);
                    font-size: 13px;
                    margin-left: 4em; 
                    margin-right: 4em;
                    margin-bottom: 0px;
                    margin-top: 0px;
                    padding: 0px;
                    color: #9F9F9F;
                    font-family: Arial;'>
                    <div style='display: flex; flex-direction: column; text-align: left'>
                        {options.FooterTitle}
                    </div>
                </div>";
        }

        public static string GeneratePageNumberFooter(PdfGenOptions options)
        {
            return $@"
                <hr color='#D4E1F0' style='margin-left: 3.0em; margin-right: 3.0em;'>
                <div style='display: flex; 
                    flex-direction: row;
                    justify-content: space-between;
                    box-sizing: border-box;
                    width: calc(100% - 8em);
                    font-size: 13px;
                    margin-left: 4em; 
                    margin-right: 4em;
                    color: #9F9F9F;
                    text-align: left;
                    font-family: Arial;'>
                    <div style='display: flex; flex-direction: column; text-align: left; text-align: left;'>
                        <b>{options.FooterTitle}</b>
                        {options.FooterName}
                    </div>

                </div>";
        }

        public static string GenerateDynamicPageNumberFooter(PdfGenOptions options)
        {
            return $@"
                <hr color='#D4E1F0' style='margin-left: 3.0em; margin-right: 3.0em;'>
                <div style='display: flex; 
                    flex-direction: row;
                    justify-content: space-between;
                    box-sizing: border-box;
                    width: calc(100% - 8em);
                    font-size: 13px;
                    margin-left: 4em; 
                    margin-right: 4em;
                    color: #9F9F9F;
                    text-align: left;
                    font-family: Arial;'>
                    <div style='display: flex; flex-direction: column; text-align: left; text-align: left;'>
                        <b>{options.FooterTitle}</b>
                        {options.FooterName}
                    </div>

                </div>";
        }

        public static string GenerateKANFooter(PdfGenOptions options)
        {
            return $@"<div style='margin-left:4em;margin-right:4em font-family: Arial;'>
                    <table style='margin-bottom:0.5em;color:#171A31;width:100%;'>
                        <tr>
            
                        </tr>
                    </table>
                </div>";
        }
    }
}
