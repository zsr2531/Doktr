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
    public bool IsStatic { get; set; }
    public bool IsVolatile { get; set; }
    public bool IsConstant { get; set; }
    public bool IsReadOnly { get; set; }
    public DocumentationFragmentCollection Value { get; set; } = new();

    public override FieldDocumentation Clone()
    {
        var clone = new FieldDocumentation(Name, Visibility, Type)
        {
            IsStatic = IsStatic,
            IsVolatile = IsVolatile,
            IsConstant = IsConstant,
            IsReadOnly = IsReadOnly,
            Value = Value.Clone(),
        };
        
        CopyDocumentationTo(clone);
        return clone;
    }
}