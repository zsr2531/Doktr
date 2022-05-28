using Doktr.Core.Models.Constraints;
using Doktr.Core.Models.Signatures;

namespace Doktr.Decompiler.Members;

public partial class MemberDecompiler
{
    public void VisitReferenceType(ReferenceTypeParameterConstraint constraint)
    {
        if (constraint.BaseType is null)
            _sb.Append(_enableNrt && constraint.Nullability == NullabilityKind.Nullable ? "class?" : "class");
        else
            WriteTypeSignature(constraint.BaseType);
    }

    public void VisitValueType(ValueTypeParameterConstraint constraint) =>
        _sb.Append(constraint.IsUnmanaged ? "unmanaged" : "struct");

    public void VisitNotNullType(NotNullTypeKindTypeParameterConstraint constraint) => _sb.Append("notnull");

    public void VisitDefaultType(DefaultTypeKindTypeParameterConstraint constraint) => _sb.Append("default");

    public void VisitInterface(InterfaceTypeParameterConstraint constraint) =>
        WriteTypeSignature(constraint.InterfaceType);

    public void VisitConstructor(ConstructorTypeParameterConstraint constraint) => _sb.Append("new()");
}