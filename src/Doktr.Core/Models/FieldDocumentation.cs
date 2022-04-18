using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public class FieldDocumentation : MemberDocumentation, IHasStatic, IHasReadOnly, IHasValue
{
    public FieldDocumentation(string name, MemberVisibility visibility, TypeSignature type)
        : base(name, visibility)
    {
        Type = type;
    }
    
    public TypeSignature Type { get; set; }
    public bool IsStatic { get; set; } = false;
    public bool IsVolatile { get; set; } = false;
    public bool IsConstant { get; set; } = false;
    public bool IsReadOnly { get; set; } = false;
    public DocumentationFragmentCollection Value { get; set; } = new();
}