namespace Doktr.Core.Models.Signatures;

public class NestedTypeSignature : TypeSignature
{
    public NestedTypeSignature(TypeSignature parent, TypeSignature child)
    {
        Parent = parent;
        Child = child;
    }

    public TypeSignature Parent { get; set; }
    public TypeSignature Child { get; set; }

    public override void AcceptVisitor(ITypeSignatureVisitor visitor) => visitor.VisitNested(this);

    public override NestedTypeSignature Clone() => new(Parent.Clone(), Child.Clone());
}