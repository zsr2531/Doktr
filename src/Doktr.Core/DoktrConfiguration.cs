using System.Diagnostics.CodeAnalysis;

namespace Doktr.Core;

[ExcludeFromCodeCoverage]
public class DoktrConfiguration
{
    public DoktrTarget[] InputFiles { get; set; } = Array.Empty<DoktrTarget>();
    public bool EnableNrt { get; set; } = true;
    public string[] XmlDocProbePaths { get; set; } = Array.Empty<string>();
    public string[] XrefUrls { get; set; } = Array.Empty<string>();
}