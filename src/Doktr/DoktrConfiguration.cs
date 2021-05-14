using System;
using System.Linq;
using System.Xml.Serialization;
using Doktr.CommandLine;

namespace Doktr
{
    [Serializable]
    [XmlType(TypeName = "Configuration")]
    public class DoktrConfiguration
    {
        [XmlIgnore]
        internal string Source
        {
            get;
            set;
        } = "";
        
        public string Root
        {
            get;
            init;
        } = ".";
        
        public DoktrTarget[] InputFiles
        {
            get;
            init;
        } = Array.Empty<DoktrTarget>();
        
        [XmlArrayItem("Directory")]
        public string[] AdditionalIncludes
        {
            get;
            init;
        } = Array.Empty<string>();

        public string OutputPath
        {
            get;
            init;
        } = "_out";

        public bool UseTablesForMethodParameters
        {
            get;
            init;
        } = false;

        [XmlArrayItem("Url")]
        public string[] XrefUrls
        {
            get;
            init;
        } = Array.Empty<string>();

        public DoktrConfiguration WithCommandLine(CommandLineParseResult cli)
        {
            return new()
            {
                Source = Source,
                Root = cli.GetOption(CommandLineSwitchProvider.Root) ?? Root,
                InputFiles = CombineTargets(InputFiles, cli.GetOption(CommandLineSwitchProvider.InputFiles) ?? ""),
                AdditionalIncludes = CombineStrings(AdditionalIncludes, cli.GetOption(CommandLineSwitchProvider.AdditionalIncludes) ?? ""),
                OutputPath = cli.GetOption(CommandLineSwitchProvider.OutputPath) ?? OutputPath,
                UseTablesForMethodParameters = UseTablesForMethodParameters || cli.HasFlag(CommandLineSwitchProvider.UseTablesForParameters),
                XrefUrls = CombineStrings(XrefUrls, cli.GetOption(CommandLineSwitchProvider.XrefUrls) ?? "")
            };
        }

        private static string[] CombineStrings(string[] config, string cli)
        {
            if (cli.Length == 0)
                return config;

            string[] elements = cli.Split(';', StringSplitOptions.RemoveEmptyEntries);
            string[] combined = new string[config.Length + elements.Length];
            Array.Copy(config, 0, combined, 0, config.Length);
            Array.Copy(elements, 0, combined, config.Length - 1, elements.Length);

            return combined;
        }

        private static DoktrTarget[] CombineTargets(DoktrTarget[] config, string cli)
        {
            if (cli.Length == 0)
                return config;

            DoktrTarget[] elements = cli.Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(':'))
                .Select(x => new DoktrTarget
                {
                    Assembly = x[0],
                    XmlFile = x[1]
                })
                .ToArray();
            DoktrTarget[] combined = new DoktrTarget[config.Length + elements.Length];
            Array.Copy(config, 0, combined, 0, config.Length);
            Array.Copy(elements, 0, combined, config.Length - 1, elements.Length);

            return combined;
        }
    }
}