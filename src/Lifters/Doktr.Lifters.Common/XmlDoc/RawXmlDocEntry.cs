using Doktr.Core.Models;
using Doktr.Core.Models.Collections;

namespace Doktr.Lifters.Common.XmlDoc;

public class RawXmlDocEntry
{
    public RawXmlDocEntry(CodeReference codeReference)
    {
        CodeReference = codeReference;
    }

    public CodeReference CodeReference { get; set; }
    public bool InheritsDocumentationImplicitly { get; set; } = false;
    public CodeReference? InheritsDocumentationExplicitlyFrom { get; set; } = null;
    public bool InheritsDocumentation => InheritsDocumentationImplicitly | InheritsDocumentationExplicitlyFrom.HasValue;
    public DocumentationFragmentCollection Summary { get; set; } = new();
    // Note that the collection holds no type information whatsoever, it will be resolved later.
    public TypeParameterDocumentationCollection TypeParameters { get; set; } = new();
    // Same applies to the parameters.
    public ParameterDocumentationCollection Parameters { get; set; } = new();
    public DocumentationFragmentCollection Value { get; set; } = new();
    public DocumentationFragmentCollection Returns { get; set; } = new();
    public ExceptionSegmentCollection Exceptions { get; set; } = new();
    public DocumentationFragmentCollection Examples { get; set; } = new();
    public DocumentationFragmentCollection Remarks { get; set; } = new();
    public MethodCodeReferenceCollection SeeAlso { get; set; } = new();
    public ProductVersionsSegmentCollection AppliesTo { get; set; } = new();
}