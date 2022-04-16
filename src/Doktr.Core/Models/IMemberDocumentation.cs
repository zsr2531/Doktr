using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public interface IMemberDocumentation
{
    string Source { get; }
    DocumentationFragmentCollection Summary { get; set; }
    DocumentationFragmentCollection Examples { get; set; }
    DocumentationFragmentCollection Remarks { get; set; }
    ProductVersionsSegmentCollection AppliesTo { get; set; }
}