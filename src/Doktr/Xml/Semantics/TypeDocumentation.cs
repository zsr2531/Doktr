using System.Collections.Immutable;

namespace Doktr.Xml.Semantics
{
    public class TypeDocumentation
    {
        public TypeDocumentation(string assembly, string ns, string name)
        {
            Assembly = assembly;
            Namespace = ns;
            Name = name;
        }
        
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
        
        public ImmutableArray<IXmlDocSegment> Summary
        {
            get;
            init;
        }

        public string Source
        {
            get;
            init;
        }
        
        public ImmutableArray<string> Inheritance
        {
            get;
            init;
        }

        public ImmutableArray<(string Name, ImmutableArray<IXmlDocSegment> Documentation)> TypeParameters
        {
            get;
            init;
        }
        
        public ImmutableArray<EventDocumentation> StaticEvents
        {
            get;
            init;
        }
        
        public ImmutableArray<FieldDocumentation> StaticFields
        {
            get;
            init;
        }
        
        public ImmutableArray<PropertyDocumentation> StaticProperties
        {
            get;
            init;
        }
        
        public ImmutableArray<MethodDocumentation> StaticMethods
        {
            get;
            init;
        }

        public ImmutableArray<EventDocumentation> InstanceEvents
        {
            get;
            init;
        }

        public ImmutableArray<FieldDocumentation> InstanceFields
        {
            get;
            init;
        }

        public ImmutableArray<MethodDocumentation> Constructors
        {
            get;
            init;
        }

        public ImmutableArray<PropertyDocumentation> InstanceProperties
        {
            get;
            init;
        }
        
        public ImmutableArray<MethodDocumentation> InstanceMethods
        {
            get;
            init;
        }
        
        public ImmutableArray<IXmlDocSegment> Remarks
        {
            get;
            init;
        }
    }
}