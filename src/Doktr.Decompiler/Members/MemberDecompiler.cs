using System.Text;
using Doktr.Core;
using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Segments;
using Doktr.Core.Models.Signatures;
using MediatR;

namespace Doktr.Decompiler.Members;

public partial class MemberDecompiler : IDocumentationMemberVisitor
{
    private readonly StringBuilder _sb = new();
    private readonly IMediator _mediator;

    public MemberDecompiler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override string ToString() => _sb.ToString();

    private void WriteVisibility(MemberDocumentation member) => WriteVisibility(member.Visibility);

    private void WriteVisibility(MemberVisibility visibility)
    {
        _sb.Append(visibility switch
        {
            MemberVisibility.Private => "private ",
            MemberVisibility.ProtectedAndAssembly => "private protected ",
            MemberVisibility.Assembly => "internal ",
            MemberVisibility.Protected => "protected ",
            MemberVisibility.ProtectedOrAssembly => "protected internal ",
            MemberVisibility.Public => "public ",
            _ => throw new ArgumentOutOfRangeException(nameof(visibility))
        });
    }

    private void WriteStatic(IHasStatic member)
    {
        if (member.IsStatic)
            _sb.Append("static ");
    }

    private void WriteVirtual(IHasVirtual member)
    {
        if (member.IsSealed)
            _sb.Append("sealed ");

        if (member.IsOverride)
            _sb.Append("override ");
        else if (member.IsVirtual)
            _sb.Append("virtual ");
        else if (member.IsAbstract)
            _sb.Append("abstract ");
    }

    private void WriteTypeParameters(IHasTypeParameters member)
    {
        var typeParameters = member.TypeParameters;
        if (typeParameters.IsEmpty())
            return;

        _sb.Append('<');

        for (int i = 0; i < typeParameters.Count; i++)
        {
            var current = typeParameters[i];
            var variance = current.Variance;
            _sb.Append(variance switch
            {
                TypeArgumentVarianceKind.Covariant => "out ",
                TypeArgumentVarianceKind.Contravariant => "in ",
                _ => string.Empty
            });

            _sb.Append(current.Name);

            if (i + 1 < typeParameters.Count)
                _sb.Append(", ");
        }

        _sb.Append('>');
    }

    private void WriteParameters(IHasParameters member, char opening = '(', char closing = ')')
    {
        var parameters = member.Parameters;
        _sb.Append(opening);

        for (int i = 0; i < parameters.Count; i++)
        {
            var parameter = parameters[i];
            WriteParameterModifier(parameter);
            string type = DecompileTypeSignature(parameter.Type);
            _sb.Append(type);

            _sb.Append(' ');
            _sb.Append(parameter.Name);
            WriteDefaultValue(parameter);

            if (i + 1 < parameters.Count)
                _sb.Append(", ");
        }

        _sb.Append(closing);

        void WriteParameterModifier(ParameterDocumentation parameter)
        {
            if (parameter.IsIn)
                _sb.Append("in ");
            else if (parameter.IsOut)
                _sb.Append("out ");
            else if (parameter.IsRef)
                _sb.Append("ref ");
            else if (parameter.IsParams)
                _sb.Append("params ");
        }

        void WriteDefaultValue(ParameterDocumentation parameter)
        {
            if (!parameter.HasDefaultValue)
                return;

            _sb.Append(" = ");
            // TODO: This needs to be refactored to allow proper formatting.
            _sb.Append(parameter.DefaultValue ?? "null");
        }
    }

    private void WriteTypeParameterConstraints(IHasTypeParameters member)
    {
        var typeParameters = member.TypeParameters;
        foreach (var parameter in typeParameters.Where(p => !p.Constraints.IsEmpty()))
        {
            _sb.Append("\n    where ");
            _sb.Append(parameter.Name);
            _sb.Append(" : ");

            var constraints = parameter.Constraints;
            constraints.AssertAtMostOneTypeKindConstraint();

            for (int i = 0; i < constraints.Count; i++)
            {
                var current = constraints[i];
                WriteTypeParameterConstraint(current);

                if (i + 1 < constraints.Count)
                    _sb.Append(", ");
            }
        }
    }

    private void WriteTypeParameterConstraint(TypeParameterConstraint constraint)
    {
        switch (constraint)
        {
            case ReferenceTypeParameterConstraint { BaseType: null } refType:
                _sb.Append(refType.Nullability switch
                {
                    NullabilityKind.NullOblivious or NullabilityKind.NotNullable => "class",
                    NullabilityKind.Nullable => "class?",
                    _ => throw new ArgumentOutOfRangeException(nameof(constraint))
                });
                break;

            case ReferenceTypeParameterConstraint { BaseType: { } baseType }:
                _sb.Append(DecompileTypeSignature(baseType));
                break;

            case ValueTypeParameterConstraint valType:
                _sb.Append(valType.IsUnmanaged ? "unmanaged" : "struct");
                break;

            case InterfaceTypeParameterConstraint infType:
                _sb.Append(DecompileTypeSignature(infType.InterfaceType));
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(constraint));
        }
    }

    private void WriteReadOnly(IHasReadOnly member)
    {
        if (member.IsReadOnly)
            _sb.Append("readonly ");
    }

    private string DecompileTypeSignature(TypeSignature signature)
    {
        var request = new DecompileTypeSignature(signature);
        var task = _mediator.Send(request);

        return task.WaitForResult();
    }
}