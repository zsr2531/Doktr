namespace Doktr.Core;

public class DoktrConfiguration
{
    public bool EnableNrt { get; set; }
    public string[] XmlDocProbePaths { get; set; } = Array.Empty<string>();
    public string[] XrefUrls { get; set; } = Array.Empty<string>();
}