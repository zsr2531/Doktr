using System.Collections.Immutable;

namespace Doktr.Xml.Semantics
{
    public class TypeDocumentation
    {
        public TypeDocumentation(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
        }
        
        public IDocumentationElement Summary
        {
            get;
            init;
        }

        public string Source
        {
            get;
            init;
        }
        
        public ImmutableArray<IDocumentationElement> Inheritance
        {
            get;
            init;
        }

        public IDocumentationElement TypeParameters
        {
            get;
            init;
        }
        
        public IDocumentationElement Remarks
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
    }
}