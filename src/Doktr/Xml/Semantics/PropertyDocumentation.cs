using System.Collections.Immutable;

namespace Doktr.Xml.Semantics
{
    public class PropertyDocumentation
    {
        public PropertyDocumentation(string name)
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
        
        public ImmutableArray<IXmlDocSegment> Remarks
        {
            get;
            init;
        }
    }
}