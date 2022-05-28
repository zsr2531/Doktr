using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Signatures;

public class ValueTupleTypeSignature : TypeSignature
{
    public TypeSignatureCollection Parameters { get; set; } = new();

    public override void AcceptVisitor(ITypeSignatureVisitor visitor) => visitor.VisitValueTuple(this);

    public override ValueTupleTypeSignature Clone() => new()
    {
        Parameters = Parameters.Clone()
    };
}