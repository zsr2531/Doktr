using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models
{
    public abstract class TypeDocumentation : IMemberDocumentation, IHasGenericParameters
    {
        protected TypeDocumentation(string assembly, string ns, string name)
        {
            Assembly = assembly;
            Namespace = ns;
            Name = name;
        }

        protected TypeDocumentation(TypeDocumentation other)
        {
            Assembly = other.Assembly;
            Namespace = other.Namespace;
            Name = other.Name;
            Summary = other.Summary;
            Syntax = other.Syntax;
            GenericParameters = other.GenericParameters;
            StaticEvents = other.StaticEvents;
            StaticFields = other.StaticFields;
            StaticProperties = other.StaticProperties;
            StaticMethods = other.StaticMethods;
            Constructors = other.Constructors;
            InstanceEvents = other.InstanceEvents;
            InstanceFields = other.InstanceFields;
            InstanceProperties = other.InstanceProperties;
            InstanceMethods = other.InstanceMethods;
            Operators = other.Operators;
            Examples = other.Examples;
            ExtensionMethods = other.ExtensionMethods;
            Remarks = other.Remarks;
        }

        public string FullName => $"{Namespace}.{Name}";
        
        public string Assembly
        {
            get;
        }

        public string Namespace
        {
            get;
        }

        public string Name
        {
            get;
        }

        public ImmutableArray<IDocumentationSegment> Summary
        {
            get;
            init;
        } = ImmutableArray<IDocumentationSegment>.Empty;

        public string? Syntax
        {
            get;
            init;
        }

        public ImmutableArray<GenericParameterDocumentation> GenericParameters
        {
            get;
            init;
        } = ImmutableArray<GenericParameterDocumentation>.Empty;

        public ImmutableArray<EventDocumentation> StaticEvents
        {
            get;
            init;
        } = ImmutableArray<EventDocumentation>.Empty;

        public ImmutableArray<FieldDocumentation> StaticFields
        {
            get;
            init;
        } = ImmutableArray<FieldDocumentation>.Empty;
        
        public ImmutableArray<PropertyDocumentation> StaticProperties
        {
            get;
            init;
        } = ImmutableArray<PropertyDocumentation>.Empty;
        
        public ImmutableArray<MethodDocumentation> StaticMethods
        {
            get;
            init;
        } = ImmutableArray<MethodDocumentation>.Empty;
        
        public ImmutableArray<ConstructorDocumentation> Constructors
        {
            get;
            init;
        } = ImmutableArray<ConstructorDocumentation>.Empty;
        
        public ImmutableArray<EventDocumentation> InstanceEvents
        {
            get;
            init;
        } = ImmutableArray<EventDocumentation>.Empty;

        public ImmutableArray<FieldDocumentation> InstanceFields
        {
            get;
            init;
        } = ImmutableArray<FieldDocumentation>.Empty;
        
        public ImmutableArray<PropertyDocumentation> InstanceProperties
        {
            get;
            init;
        } = ImmutableArray<PropertyDocumentation>.Empty;
        
        public ImmutableArray<MethodDocumentation> InstanceMethods
        {
            get;
            init;
        } = ImmutableArray<MethodDocumentation>.Empty;
        
        public ImmutableArray<MethodDocumentation> Operators
        {
            get;
            init;
        } = ImmutableArray<MethodDocumentation>.Empty;

        public ImmutableArray<IDocumentationSegment> Examples
        {
            get;
            init;
        } = ImmutableArray<IDocumentationSegment>.Empty;
        
        public ImmutableArray<MemberReference> ExtensionMethods
        {
            get;
            init;
        } = ImmutableArray<MemberReference>.Empty;

        public ImmutableArray<IDocumentationSegment> Remarks
        {
            get;
            init;
        } = ImmutableArray<IDocumentationSegment>.Empty;

        public ImmutableArray<IReference> SeeAlso
        {
            get;
            init;
        } = ImmutableArray<IReference>.Empty;
    }
}