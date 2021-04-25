namespace Doktr.Xml.Semantics
{
    public class EventDocumentation
    {
        public EventDocumentation(string name)
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
        
        public IDocumentationElement Type
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