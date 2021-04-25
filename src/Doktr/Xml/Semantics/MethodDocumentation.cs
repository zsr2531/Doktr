using System.Collections.Immutable;

namespace Doktr.Xml.Semantics
{
    public class MethodDocumentation
    {
        public MethodDocumentation(string name)
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
        
        public ImmutableArray<(string Name, IDocumentationElement Documentation)> TypeParameters
        {
            get;
            init;
        }

        public ImmutableArray<(IDocumentationElement Type, string Name, IDocumentationElement Documentation)> Parameters
        {
            get;
            init;
        }
        
        public IDocumentationElement Returns
        {
            get;
            init;
        }
        
        public ImmutableArray<(IDocumentationElement Exception, IDocumentationElement Documentation)> Exceptions
        {
            get;
            init;
        }
        
        public IDocumentationElement Remarks
        {
            get;
            init;
        }
    }
}