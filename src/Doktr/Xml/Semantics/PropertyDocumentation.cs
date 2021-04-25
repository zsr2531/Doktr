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
        
        public IDocumentationElement Remarks
        {
            get;
            init;
        }
    }
}