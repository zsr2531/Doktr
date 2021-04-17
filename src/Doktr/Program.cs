using System;
using System.IO;
using System.Linq;
using System.Text;
using AsmResolver.DotNet;
using Doktr.Analysis;
using Doktr.CommandLine;
using Doktr.Generation;
using Doktr.Resolution;
using Doktr.Xml;

namespace Doktr
{
    public static class Program
    {
        private const string Version = "1.0.0.0";
        private const string RepoUrl = "https://github.com/zsr2531/Doktr";
        private const string Usage = "dotnet Doktr.dll [options] <asm:xml>...";
            
        public static void Main(string[] args)
        {
            var arguments = new CommandLineParser(args).Parse();
            if (arguments.HasFlag(CommandLineSwitches.Help))
            {
                PrintHelpMessage();
                return;
            }

            foreach (var target in arguments.TargetFiles)
            {
                var module = ModuleDefinition.FromFile(target.Assembly);
                var result = new DependencyGraphBuilder(module).BuildDependencyGraph();
                var docs = new DocumentationReader(target.Xmldoc);
                var documentation = new DocumentationResolver(result, docs.Loaded).MapMembers();
                new InheritDocResolver(documentation, result, docs.Loaded).ResolveInheritDoc();

                var fs = File.OpenWrite("output.md");
                var writer = new StreamWriter(fs);
                var visitor = new MarkdownDocumentationVisitor(writer);
                var generator = new DocumentationGenerator(visitor, documentation);

                var meme = documentation.Keys.ElementAt(42);
                writer.WriteLine("```"+meme.FullName+"```");
                generator.GenerateDocumentation(meme);

                writer.Flush();
                fs.Flush();
                fs.Close();
            }
        }

        private static void PrintHelpMessage()
        {
            var sb = new StringBuilder($"Doktr v{Version}\n{RepoUrl}\n\nUSAGE: {Usage}\n\nCommand line options:\n");

            foreach (var @switch in CommandLineSwitches.Switches)
            {
                string identifiers = "   " + string.Join(", ", @switch.Identifiers);
                sb.Append(identifiers.PadRight(25));
                sb.Append(@switch.Description);

                if (@switch.HasValue)
                    sb.AppendLine($" (default: {@switch.DefaultValue})");
                else
                    sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
        }
    }
}