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

        public ImmutableArray<FieldDocumentation> StaticProperties
        {
            get;
            init;
        } = ImmutableArray<FieldDocumentation>.Empty;
        
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

        public ImmutableArray<FieldDocumentation> InstanceProperties
        {
            get;
            init;
        } = ImmutableArray<FieldDocumentation>.Empty;
        
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
        
        public ImmutableArray<TypeDocumentation> NestedTypes
        {
            get;
            init;
        } = ImmutableArray<TypeDocumentation>.Empty;

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