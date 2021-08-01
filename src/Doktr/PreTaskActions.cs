using System;
using System.IO;
using System.Xml.Serialization;
using Doktr.CommandLine;

namespace Doktr
{
    public static class PreTaskActions
    {
        public static void RunActionsIfNeeded(CommandLineParseResult cli)
        {
            if (cli.HasFlag(CommandLineSwitchProvider.Help))
                PrintHelpMessage();
            else if (cli.HasFlag(CommandLineSwitchProvider.About))
                PrintAboutMessage();
            else if (cli.HasFlag(CommandLineSwitchProvider.GenerateExample))
                GenerateExample();
            else
                return;

            Environment.Exit(0);
        }

        private static void PrintHelpMessage()
        {
            Console.WriteLine("USAGE: ./Doktr [SWITCHES]... [FILE]...\n");
            Console.WriteLine("Valid command line switches:");

            var switches = CommandLineSwitchProvider.Instance;

            foreach (var sw in switches.AllSwitches)
            {
                string identifiers = "   " + string.Join(" ", sw.Identifiers).PadRight(25);
                Console.Write(identifiers);
                Console.Write(sw.Description);
                
                if (sw.DefaultValue is { Length: >0 } value)
                    Console.Write($" (default: {value})");
                
                Console.WriteLine();
            }
            
            Console.WriteLine("\nExamples:");
            Console.WriteLine("./Doktr --use-tables docs.xml");
            Console.WriteLine("./Doktr --include docs/articles;docs/samples -o docs/_site -if bin/Project1/Project1.dll:bin/Project1/Project1.xml");
        }

        private static void PrintAboutMessage()
        {
            Console.WriteLine("Doktr v0.0.1\n");
            Console.WriteLine("https://github.com/zsr2531/Doktr.git");
            Console.WriteLine("Doktr is licensed under the MIT license.\n");
            
            Console.WriteLine("External libraries:");
            Console.WriteLine("AsmResolver (MIT license): https://github.com/Washi1337/AsmResolver.git");
            Console.WriteLine("serilog (Apache 2.0 license): https://github.com/serilog/serilog.git");
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
                    "docs/articles/",
                    "docs/examples/"
                },
                OutputPath = "docs/_site",
                UseTablesForMethodParameters = false,
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