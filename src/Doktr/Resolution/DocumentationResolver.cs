using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AsmResolver.DotNet;
using Doktr.Analysis;
using Doktr.Xml;

namespace Doktr.Resolution
{
    public class DocumentationResolver
    {
        private readonly DependencyGraph _dependencyGraph;
        private readonly IReadOnlyDictionary<string, ImmutableArray<IXmlDocSegment>> _documentation;

        private static readonly TypeSignatureVisitor Normal = new(false);
        private static readonly TypeSignatureVisitor Param = new(true);

        public DocumentationResolver(
            DependencyGraph dependencyGraph,
            IReadOnlyDictionary<string, ImmutableArray<IXmlDocSegment>> documentation)
        {
            _dependencyGraph = dependencyGraph;
            _documentation = documentation;
        }

        public IDictionary<IFullNameProvider, ImmutableArray<IXmlDocSegment>> MapMembers()
        {
            var dictionary = new Dictionary<IFullNameProvider, ImmutableArray<IXmlDocSegment>>();

            foreach (var (_, node) in _dependencyGraph.Nodes)
            {
                if (node == _dependencyGraph.Root)
                    continue;
                
                string key = Transform(node.MetadataMember);
                if (!_documentation.TryGetValue(key, out var doc))
                    continue;

                dictionary.Add(node.MetadataMember, doc);
            }

            return dictionary;
        }

        private static string Transform(IFullNameProvider fullNameProvider)
        {
            if (fullNameProvider is TypeDefinition type)
                return $"T:{type.FullName}";

            var member = (IMemberDefinition) fullNameProvider;
            var parent = member.DeclaringType;
            string name = member.Name.Replace('.', '#');

            if (member is PropertyDefinition { GetMethod: { Parameters: { Count: 0 } } } or EventDefinition or FieldDefinition)
                return $"{Prefix(member)}:{parent.FullName}.{name}";

            var method = member is MethodDefinition definition
                ? definition
                : ((PropertyDefinition) member).GetMethod;
            if (method.Parameters.Count == 0)
                return $"M:{parent.FullName}.{name}";

            string generic = method.GenericParameters.Count == 0 ? "" : "``" + method.GenericParameters.Count;
            string parameters = string.Join(",", method.Parameters.Select(p => p.ParameterType.AcceptVisitor(Param)));
            if (method.Name is not "op_Implicit" and not "op_Explicit")
                return $"{Prefix(fullNameProvider)}:{parent.FullName}.{name}{generic}({parameters})";
            
            return $"M:{parent.FullName}.{name}{generic}({parameters})~{method.Signature.ReturnType.AcceptVisitor(Normal)}";
        }

        private static char Prefix(IFullNameProvider fullNameProvider)
        {
            return fullNameProvider switch
            {
                TypeDefinition => 'T',
                MethodDefinition => 'M',
                FieldDefinition => 'F',
                PropertyDefinition => 'P',
                EventDefinition => 'E',
                _ => throw new ArgumentOutOfRangeException(nameof(fullNameProvider))
            };
        }
    }
}