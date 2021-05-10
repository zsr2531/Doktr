using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Doktr.CommandLine;

namespace Doktr
{
    public static class PreTaskActions
    {
        private static readonly HashSet<CommandLineSwitch> Flags = new()
        {
            CommandLineSwitchProvider.Instance.About,
            CommandLineSwitchProvider.Instance.Help,
            CommandLineSwitchProvider.Instance.GenerateExample
        };
        
        public static void RunActionsIfNeeded(CommandLineParseResult cli)
        {
            if (!AnyActionsNeeded(cli))
                return;
            
            if (cli.HasFlag(CommandLineSwitchProvider.Instance.About))
                PrintHelpMessage();
            else if (cli.HasFlag(CommandLineSwitchProvider.Instance.Help))
                PrintAboutMessage();
            else
                GenerateExample();

            Environment.Exit(0);
        }

        private static bool AnyActionsNeeded(CommandLineParseResult cli)
        {
            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (var flag in Flags)
            {
                if (cli.HasFlag(flag))
                    return true;
            }

            return false;
        }

        private static void PrintHelpMessage()
        {
        }

        private static void PrintAboutMessage()
        {
        }

        private static void GenerateExample()
        {
            if (File.Exists("example.xml"))
            {
                Console.Error.WriteLine("There is already a file named 'example.xml' in the current directory.");
                Console.Error.WriteLine("Please rename it or remove it first.");
                return;
            }

            var configuration = new DoktrConfiguration
            {
                Root = "../",
                InputFiles = new DoktrTarget[]
                {
                    new()
                    {
                        Assembly = "src/Project1/bin/Debug/net5.0/Project1.dll",
                        XmlFile = "src/Project1/bin/Debug/net5.0/Project1.xml"
                    },
                    new()
                    {
                        Assembly = "src/Project2/bin/Debug/net5.0/Project2.dll",
                        XmlFile = "src/Project2/bin/Debug/net5.0/Project2.xml"
                    }
                },
                AdditionalIncludes = new[]
                {
                    "docs/articles",
                    "docs/copyright"
                },
                OutputPath = "docs/_site",
                UseTablesForMethodParameters = true,
                XrefUrls = new[]
                {
                    "https://xref.docs.microsoft.com/query?uid={uid}"
                }
            };

            using var stream = File.Open("example.xml", FileMode.Create);
            var xml = new XmlSerializer(typeof(DoktrConfiguration));
            xml.Serialize(stream, configuration);
            
            Console.WriteLine("An example has been generated and written to 'example.xml'.");
        }
    }
}