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
        
        public ImmutableArray<(string Name, ImmutableArray<IXmlDocSegment> Documentation)> TypeParameters
        {
            get;
            init;
        }

        public ImmutableArray<(IXmlDocSegment Type, string Name, ImmutableArray<IXmlDocSegment> Documentation)> Parameters
        {
            get;
            init;
        }
        
        public ImmutableArray<IXmlDocSegment> Returns
        {
            get;
            init;
        }
        
        public ImmutableArray<(ImmutableArray<IXmlDocSegment> Exception, ImmutableArray<IXmlDocSegment> Documentation)> Exceptions
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