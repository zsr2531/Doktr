using CommandLine;

namespace Doktr;

public class CommandLineOptions
{
    [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.", Default = false)]
    public bool Verbose { get; set; }

    [Value(0, Required = true, HelpText = "The path to the project file.", MetaName = "path")]
    public string ProjectFilePath { get; set; } = null!;
}