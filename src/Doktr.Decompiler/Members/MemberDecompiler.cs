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

    public void VisitEvent(EventDocumentation eventDocumentation) => throw new NotImplementedException();

    public void VisitField(FieldDocumentation fieldDocumentation) => throw new NotImplementedException();

    public void VisitConstructor(ConstructorDocumentation constructorDocumentation) =>
        throw new NotImplementedException();

    public void VisitFinalizer(FinalizerDocumentation finalizerDocumentation) => throw new NotImplementedException();

    public void VisitIndexer(IndexerDocumentation indexerDocumentation) => throw new NotImplementedException();

    public void VisitProperty(PropertyDocumentation propertyDocumentation) => throw new NotImplementedException();

    public void VisitMethod(MethodDocumentation methodDocumentation) => throw new NotImplementedException();

    public void VisitOperator(OperatorDocumentation operatorDocumentation) => throw new NotImplementedException();

    public void VisitConversionOperator(ConversionOperatorDocumentation conversionOperatorDocumentation) =>
        throw new NotImplementedException();

    public override string ToString() => _sb.ToString();

    private void WriteVisibility(MemberDocumentation member)
    {
        _sb.Append(member.Visibility switch
        {
            MemberVisibility.Private => "private ",
            MemberVisibility.ProtectedAndAssembly => "private protected ",
            MemberVisibility.Assembly => "internal ",
            MemberVisibility.Protected => "protected ",
            MemberVisibility.ProtectedOrAssembly => "protected internal ",
            MemberVisibility.Public => "public ",
            _ => throw new ArgumentOutOfRangeException(nameof(member))
        });
    }

    private void WriteTypeParameters(IHasTypeParameters member)
    {
        var typeParameters = member.TypeParameters;
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

    private void WriteParameters(IHasParameters member)
    {
        var parameters = member.Parameters;
        _sb.Append('(');

        for (int i = 0; i < parameters.Count; i++)
        {
            var current = parameters[i];
            WriteParameterModifier(current);

            string type = DecompileTypeSignature(current.Type);
            _sb.Append(type);
            _sb.Append(' ');
            _sb.Append(current.Name);

            WriteDefaultValue(current);

            if (i + 1 < parameters.Count)
                _sb.Append(", ");
        }

        _sb.Append(')');

        void WriteParameterModifier(ParameterSegment parameter)
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

        void WriteDefaultValue(ParameterSegment parameter)
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

    private string DecompileTypeSignature(TypeSignature signature)
    {
        var request = new DecompileTypeSignature(signature);
        var task = _mediator.Send(request);

        return task.WaitForResult();
    }
}