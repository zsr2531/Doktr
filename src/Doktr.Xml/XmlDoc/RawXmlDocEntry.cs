using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Xml.XmlDoc.Collections;

namespace Doktr.Xml.XmlDoc;

public class RawXmlDocEntry
{
    public RawXmlDocEntry(CodeReference docId)
    {
        DocId = docId;
    }

    public bool InheritsDocumentation => InheritsDocumentationImplicitly | InheritsDocumentationExplicitlyFrom.HasValue;
    public CodeReference DocId { get; set; }
    public bool InheritsDocumentationImplicitly { get; set; } = false;
    public CodeReference? InheritsDocumentationExplicitlyFrom { get; set; } = null;
    public DocumentationFragmentCollection Summary { get; set; } = new();
    public NamedDocumentationMap TypeParameters { get; set; } = new();
    public NamedDocumentationMap Parameters { get; set; } = new();
    public DocumentationFragmentCollection Value { get; set; } = new();
    public DocumentationFragmentCollection Returns { get; set; } = new();
    public ExceptionDocumentationMap Exceptions { get; set; } = new();
    public DocumentationFragmentCollection Example { get; set; } = new();
    public DocumentationFragmentCollection Remarks { get; set; } = new();
    public LinkDocumentationFragmentCollection SeeAlso { get; set; } = new();
    public ProductVersionsSegmentCollection AppliesTo { get; set; } = new(); // TODO: Figure out how to parse this.
}