using System.Text;
using Doktr.Core;
using Doktr.Core.Models;
using Doktr.Core.Models.Signatures;
using MediatR;

namespace Doktr.Decompiler.Members;

internal partial class MemberDecompiler : IDocumentationMemberVisitor
{
    private readonly StringBuilder _sb = new();
    private readonly IMediator _mediator;

    internal MemberDecompiler(IMediator mediator)
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

    private void WriteTypeParameters(IHasTypeParameters member)
    {
        var typeParameters = member.TypeParameters;
        var names = typeParameters.Select(t => t.Name);
        string joinedNames = string.Join(", ", names);

        _sb.Append('<');
        _sb.Append(joinedNames);
        _sb.Append('>');
    }

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
            _ => throw new ArgumentOutOfRangeException(nameof(member.Visibility))
        });
    }

    private string DecompileTypeSignature(TypeSignature signature)
    {
        var request = new DecompileTypeSignature(signature);
        var task = _mediator.Send(request);

        return task.WaitForResult();
    }
}