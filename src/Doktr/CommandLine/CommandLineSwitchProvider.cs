using System.Collections.Immutable;

namespace Doktr.CommandLine
{
    public class CommandLineSwitchProvider : ICommandLineSwitchProvider
    {
        public static readonly CommandLineSwitchProvider Instance = new();

        private CommandLineSwitchProvider()
        {
            var flagsBuilder = ImmutableDictionary.CreateBuilder<string, CommandLineSwitch>();
            var optionsBuilder = ImmutableDictionary.CreateBuilder<string, CommandLineSwitch>();
            
            RegisterSwitch(Help, flagsBuilder);
            RegisterSwitch(Verbose, flagsBuilder);
            RegisterSwitch(About, flagsBuilder);
            RegisterSwitch(GenerateExample, flagsBuilder);
            
            RegisterSwitch(AdditionalIncludes, optionsBuilder);
            RegisterSwitch(OutputPath, optionsBuilder);
            RegisterSwitch(UseTablesForParameters, optionsBuilder);
            RegisterSwitch(XrefUrls, optionsBuilder);

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
        
        public ImmutableDictionary<string, CommandLineSwitch> Flags
        {
            get;
        }
        
        public ImmutableDictionary<string, CommandLineSwitch> Options
        {
            get;
        }

        public CommandLineSwitch AdditionalIncludes
        {
            get;
        } = new("Sets the paths to copy the contents from to the output directory.", "", "--include");

        public CommandLineSwitch OutputPath
        {
            get;
        } = new("Sets the output directory path.", "_out", "-o", "--out");

        public CommandLineSwitch UseTablesForParameters
        {
            get;
        } = new("Sets whether the resulting markdown should use tables for parameters.", null, "--use-tables");

        public CommandLineSwitch XrefUrls
        {
            get;
        } = new("Sets the URLs to retrieve external references from.", "", "--external-xref");

        public CommandLineSwitch GenerateExample
        {
            get;
        } = new("Generates an example config to 'example.xml'.", null, "--generate-example");
        
        public CommandLineSwitch Verbose
        {
            get;
        } = new("Shows more output.", null, "--verbose");

        public CommandLineSwitch Help
        {
            get;
        } = new("Shows this help message.", null, "-h", "--help");

        public CommandLineSwitch About
        {
            get;
        } = new("Shows copyright and additional information.", null, "--about", "--version");
    }
}