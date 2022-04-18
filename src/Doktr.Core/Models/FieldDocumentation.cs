using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public class FieldDocumentation : MemberDocumentation, IHasValue
{
    public FieldDocumentation(string name, MemberVisibility visibility, TypeSignature type)
        : base(name, visibility)
    {
        Type = type;
    }
    
    public TypeSignature Type { get; set; }
    public DocumentationFragmentCollection Value { get; set; } = new();
}