using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Trail.Application.Services.Pdf
{
    /// \(summary\)
    /// Utilities for extracting information from PDFs, typically after we have
    /// generated them.
    /// \(/summary\)
    public static class PdfRead
    {

        public static string GetHtml(string token)
        {
            return $"<span class='Invisible'>{token}</span>";
        }

        public static string GetRegex(string token)
        {
            return String.Join(@"\s*", token.ToCharArray());
        }

        // These are characters that are not escaped in the css content property, and are not case sensitive
        public const string H1BeginToken = "#1-";
        public const string H2BeginToken = "#2-";
        public const string H3BeginToken = "#3-";
        public const string H4BeginToken = "#4-";
        public const string H5BeginToken = "#5-";
        public const string H6BeginToken = "#6-";
        public const string H1EndToken = "-1#";
        public const string H2EndToken = "-2#";
        public const string H3EndToken = "-3#";
        public const string H4EndToken = "-4#";
        public const string H5EndToken = "-5#";
        public const string H6EndToken = "-6#";
        public const string PageNumberToken = "-p#-";

        /// \(summary\)
        /// Builds headings from set of regex matches on a PDF's page for a specific heading level
        /// \(/summary\)
        /// \(param name="matches"\)The matches to turn into headings.\(/param\)
        /// \(param name="headingLevel"\)The heading level of the heading mathces.\(/param\)
        /// \(param name="pageIndex"\)The page index that the headings are on.\(/param\)
        /// \(returns\)A list of headings of the given level on a given page.\(/returns\)
        public static List<Heading> ExtractHeadingsFromMatches(MatchCollection matches, int headingLevel, int pageIndex)
        {
            return matches.Cast<Match>()
                .Select(
                    m => new Heading
                    {
                        Text = m.Value.Trim().ToLower(),
                        Level = headingLevel,
                        Index = m.Index, // WARNING: Probably starts from the beginning token.
                        Page = pageIndex
                    })
                .ToList();
        }
    }
}
