using System;
using System.Collections.Immutable;
using System.Linq;
using AsmResolver.DotNet;
using Doktr.Dependencies;
using Serilog;

namespace Doktr.Services;

public class DocumentationMapperService : IDocumentationMapperService
{
    private readonly ITypeSignatureTranslationService _translation;
    private readonly ILogger _logger;

    public DocumentationMapperService(ITypeSignatureTranslationService translation, ILogger logger)
    {
        _translation = translation;
        _logger = logger;
    }

    public ImmutableDictionary<IFullNameProvider, XmlDocEntry> MapDocumentation(
        DependencyGraph dependencyGraph,
        ImmutableDictionary<string, XmlDocEntry> documentation)
    {
        var builder = ImmutableDictionary.CreateBuilder<IFullNameProvider, XmlDocEntry>();
        var remaining = documentation.Keys.ToHashSet();

        foreach (var node in dependencyGraph.Mapping.Values)
        {
            if (node.MetadataMember is AssemblyDefinition)
                continue;

            string key = Transform(node.MetadataMember);
            if (!documentation.TryGetValue(key, out var entry))
                continue; // TODO: Warning?

            remaining.Remove(key);
            builder.Add(node.MetadataMember, entry);
        }

        foreach (string remains in remaining)
            _logger.Warning("Couldn't find the member to map the documentation with entry '{Unused}' to", remains);

        return builder.ToImmutable();
    }

    private string Transform(IFullNameProvider fullNameProvider)
    {
        if (fullNameProvider is TypeDefinition type)
            return $"T:{type.FullName}";

        var member = (IMemberDefinition) fullNameProvider;
        var parent = member.DeclaringType;
        string name = member.Name.Replace('.', '#');

        if (member is PropertyDefinition { GetMethod.Parameters.Count: 0 } or EventDefinition or FieldDefinition)
            return $"{Prefix(member)}:{parent.FullName}.{name}";

        var method = member is MethodDefinition definition
            ? definition
            : ((PropertyDefinition) member).GetMethod;
        if (method.Parameters.Count == 0)
            return $"M:{parent.FullName}.{name}";

        string generic = method.GenericParameters.Count == 0 ? "" : "``" + method.GenericParameters.Count;
        string parameters = string.Join(",", method.Parameters.Select(p => p.ParameterType.AcceptVisitor(_translation)));
        if (method.Name.Value is not "op_Implicit" and not "op_Explicit")
            return $"{Prefix(member)}:{parent.FullName}.{name}{generic}({parameters})";
            
        return $"M:{parent.FullName}.{name}{generic}({parameters})~{method.Signature.ReturnType.AcceptVisitor(_translation)}";
    }

    private char Prefix(IFullNameProvider fullNameProvider)
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