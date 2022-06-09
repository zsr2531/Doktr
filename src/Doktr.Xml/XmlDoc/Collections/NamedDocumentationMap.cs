using Doktr.Core.Models.Collections;

namespace Doktr.Xml.XmlDoc.Collections;

// TODO: Somehow deal with collisions. Some random stuff could be appended to the end of the string as a last resort.
public class NamedDocumentationMap : Dictionary<string, DocumentationFragmentCollection>
{
    private const string Missing = "MISSING#";
    private int _missing = 1;

    public new void Add(string? key, DocumentationFragmentCollection value) =>
        base.Add(key ?? GetMissingName(), value);

    private string GetMissingName() => Missing + _missing++;
}