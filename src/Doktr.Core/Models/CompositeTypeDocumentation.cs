using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public abstract class CompositeTypeDocumentation : CommonTypeCharacteristics,
    IHasStatic
{
    protected CompositeTypeDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public bool IsStatic { get; set; }

    public MemberCollection<FieldDocumentation> Fields { get; set; } = new();
    public MemberCollection<ConstructorDocumentation> Constructors { get; set; } = new();
    public MemberCollection<OperatorDocumentation> Operators { get; set; } = new();
    public MemberCollection<ConversionOperatorDocumentation> ConversionOperators { get; set; } = new();
    public ExplicitImplementationCollection ExplicitImplementations { get; set; } = new();

    public abstract override CompositeTypeDocumentation Clone();

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is CompositeTypeDocumentation otherCompositeType)
        {
            otherCompositeType.IsStatic = IsStatic;
            otherCompositeType.Interfaces = Interfaces.Clone();
            otherCompositeType.Fields = Fields.Clone();
            otherCompositeType.Constructors = Constructors.Clone();
            otherCompositeType.Operators = Operators.Clone();
            otherCompositeType.ConversionOperators = ConversionOperators.Clone();
            otherCompositeType.ExplicitImplementations = ExplicitImplementations.Clone();
        }

        base.CopyDocumentationTo(other);
    }
}