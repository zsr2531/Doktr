using System.Collections.Immutable;

namespace Doktr.CommandLine;

public class CommandLineSwitchProvider : ICommandLineSwitchProvider
{
    public static readonly CommandLineSwitchProvider Instance;

    public static CommandLineSwitch Root
    {
        get;
    } = new("Sets the root directory for paths", ".", "-r", "--root");

    public static CommandLineSwitch InputFiles
    {
        get;
    } = new("Sets the paths to .dll and .xml file pairs to generate documentation from. (format: path/to/dll:path/to/xml separate multiple inputs with ';')", "", "-if", "--input");

    public static CommandLineSwitch AdditionalIncludes
    {
        get;
    } = new("Sets the paths to copy the contents from to the output directory. (separate paths with ';')", "", "--include");

    public static CommandLineSwitch OutputPath
    {
        get;
    } = new("Sets the output directory path.", "_out", "-o", "--out");

    public static CommandLineSwitch UseTablesForParameters
    {
        get;
    } = new("Sets whether the resulting markdown should use tables for parameters.", null, "--use-tables");

    public static CommandLineSwitch XrefUrls
    {
        get;
    } = new("Sets the URLs to retrieve external references from. (separate URLs with ';')", "", "--external-xref");

    public static CommandLineSwitch GenerateExample
    {
        get;
    } = new("Generates an example config to 'example.xml'.", null, "--generate-example");
        
    public static CommandLineSwitch Verbose
    {
        get;
    } = new("Shows more output.", null, "--verbose");

    public static CommandLineSwitch Help
    {
        get;
    } = new("Shows this help message.", null, "-h", "--help");

    public static CommandLineSwitch About
    {
        get;
    } = new("Shows copyright and additional information.", null, "--about", "--version");

    static CommandLineSwitchProvider()
    {
        Instance = new();
    }

    private CommandLineSwitchProvider()
    {
        var flagsBuilder = ImmutableDictionary.CreateBuilder<string, CommandLineSwitch>();
        var optionsBuilder = ImmutableDictionary.CreateBuilder<string, CommandLineSwitch>();
            
        RegisterSwitch(Help, flagsBuilder);
        RegisterSwitch(Verbose, flagsBuilder);
        RegisterSwitch(About, flagsBuilder);
        RegisterSwitch(GenerateExample, flagsBuilder);
            
        RegisterSwitch(InputFiles, optionsBuilder);
        RegisterSwitch(AdditionalIncludes, optionsBuilder);
        RegisterSwitch(OutputPath, optionsBuilder);
        RegisterSwitch(UseTablesForParameters, optionsBuilder);
        RegisterSwitch(XrefUrls, optionsBuilder);

        AllSwitches = ImmutableArray.Create(new[]
        {
            Help,
            Verbose,
            About,
            GenerateExample,
                
            InputFiles,
            AdditionalIncludes,
            OutputPath,
            UseTablesForParameters,
            XrefUrls
        });
        Flags = flagsBuilder.ToImmutable();
        Options = optionsBuilder.ToImmutable();

        static void RegisterSwitch(
            CommandLineSwitch sw,
            ImmutableDictionary<string, CommandLineSwitch>.Builder builder)
        {
            foreach (string identifier in sw.Identifiers)
                builder.Add(identifier, sw);
        }
    }

    public ImmutableArray<CommandLineSwitch> AllSwitches
    {
        get;
    }

    public ImmutableDictionary<string, CommandLineSwitch> Flags
    {
        get;
    }
        
    public ImmutableDictionary<string, CommandLineSwitch> Options
    {
        get;
    }
}