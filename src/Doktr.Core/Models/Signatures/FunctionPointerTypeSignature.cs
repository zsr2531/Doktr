using System.Diagnostics.CodeAnalysis;
using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Signatures;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "IdentifierTypo")]
public enum CallingConventions
{
    Managed,
    Cdecl,
    Fastcall,
    Stdcall,
    Thiscall,
    MemberFunction,
    SuppressGCTransition
}

public class FunctionPointerTypeSignature : TypeSignature
{
    public CallingConventions CallingConvention { get; set; }
    public TypeSignatureCollection Parameters { get; set; } = new();

    public override void AcceptVisitor(ITypeSignatureVisitor visitor) => visitor.VisitFunctionPointer(this);

    public override FunctionPointerTypeSignature Clone() => new()
    {
        CallingConvention = CallingConvention,
        Parameters = Parameters.Clone()
    };
}