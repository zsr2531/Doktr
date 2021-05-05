#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AsmResolver.DotNet;
using Doktr.Analysis;
using Doktr.CommandLine;
using Doktr.Generation;
using Doktr.Resolution;
using Doktr.Xml;
using Doktr.Xml.Semantics;

namespace Doktr
{
    /// <summary>
    /// Test
    /// </summary>
    /// <param name="One">string</param>
    /// <param name="Two">int</param>
    public record Test(string? One, int? Two) : IEnumerable<int?>
    {
        /// <summary>
        /// kek
        /// </summary>
        /// <returns>ayy</returns>
        public IEnumerator<int?> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        // public unsafe void oops(in int x, ref int y, out int z, delegate* unmanaged[Cdecl]<int, int> kek)
        // {
        //     z = 0;
        // }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

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
                var semantics = new SemanticXmlDocParser(documentation);
                foreach (var type in result.Nodes.Where(p => p.Key is TypeDefinition))
                {
                    if (!documentation.ContainsKey(type.Key))
                        continue;
                    var parsed = semantics.ParseTypeDocumentation((TypeDefinition) type.Key);
                }
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