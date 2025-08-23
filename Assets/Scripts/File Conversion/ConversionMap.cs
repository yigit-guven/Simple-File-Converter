using System.Collections.Generic;

public static class ConversionMap
{
    // Dictionary of possible conversions
    public static readonly Dictionary<string, string[]> SupportedConversions =
        new Dictionary<string, string[]>
    {
        { "txt", new string[] { "pdf", "docx" } },
        { "pdf", new string[] { "txt", "docx" } },
        { "docx", new string[] { "pdf", "txt" } },
        { "xls", new string[] { "xlsx", "pdf" } },
        { "xlsx", new string[] { "xls", "pdf" } }
    };
}
