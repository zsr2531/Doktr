using System.Diagnostics.CodeAnalysis;

namespace Doktr.Core;

[ExcludeFromCodeCoverage]
public class DoktrConfiguration
{
    public (string AssemblyPath, string XmlPath)[] InputFiles { get; set; } = Array.Empty<(string, string)>();
    public bool EnableNrt { get; set; } = true;
    public string[] XmlDocProbePaths { get; set; } = Array.Empty<string>();
    public string[] XrefUrls { get; set; } = Array.Empty<string>();
}