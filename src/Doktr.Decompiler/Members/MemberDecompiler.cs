using System.Text;
using Doktr.Core;
using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Constants;
using Doktr.Core.Models.Constraints;
using Doktr.Core.Models.Signatures;
using MediatR;

namespace Doktr.Decompiler.Members;

public partial class MemberDecompiler : IDocumentationMemberVisitor, IConstantVisitor, ITypeParameterConstraintVisitor
{
    private readonly StringBuilder _sb = new();
    private readonly IMediator _mediator;
    private readonly bool _enableNrt;

    public MemberDecompiler(IMediator mediator, bool enableNrt = true)
    {
        _mediator = mediator;
        _enableNrt = enableNrt;
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
                TypeParameterVarianceKind.Covariant => "out ",
                TypeParameterVarianceKind.Contravariant => "in ",
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
            WriteTypeSignature(parameter.Type);

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
            parameter.DefaultValue.AcceptVisitor(this);
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
                current.AcceptVisitor(this);

                if (i + 1 < constraints.Count)
                    _sb.Append(", ");
            }
        }
    }

    private void WriteReadOnly(IHasReadOnly member)
    {
        if (member.IsReadOnly)
            _sb.Append("readonly ");
    }

    private void WriteTypeSignature(TypeSignature signature)
    {
        string decompiled = DecompileTypeSignature();
        _sb.Append(decompiled);

        string DecompileTypeSignature()
        {
            var request = new DecompileTypeSignature(signature);
            var task = _mediator.Send(request);

            return task.WaitForResult();
        }
    }
}