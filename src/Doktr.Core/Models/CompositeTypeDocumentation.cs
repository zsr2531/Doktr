using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public abstract class CompositeTypeDocumentation : TypeDocumentation,
    IHasStatic,
    IHasAbstract,
    IHasCommonTypeCharacteristics
{
    protected CompositeTypeDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public bool IsStatic { get; set; }
    public bool IsAbstract { get; set; }
    public bool IsSealed { get; set; }
    public TypeSignatureCollection Interfaces { get; set; } = new();
    public MemberCollection<EventDocumentation> Events { get; set; } = new();
    public MemberCollection<FieldDocumentation> Fields { get; set; } = new();
    public MemberCollection<ConstructorDocumentation> Constructors { get; set; } = new();
    public MemberCollection<IndexerDocumentation> Indexers { get; set; } = new();
    public MemberCollection<PropertyDocumentation> Properties { get; set; } = new();
    public MemberCollection<MethodDocumentation> Methods { get; set; } = new();
    public MemberCollection<OperatorDocumentation> Operators { get; set; } = new();
    public MemberCollection<ConversionOperatorDocumentation> ConversionOperators { get; set; } = new();
    public ExplicitImplementationCollection ExplicitImplementations { get; set; } = new();

    public abstract override CompositeTypeDocumentation Clone();

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is CompositeTypeDocumentation otherCompositeType)
        {
            otherCompositeType.IsStatic = IsStatic;
            otherCompositeType.IsAbstract = IsAbstract;
            otherCompositeType.IsSealed = IsSealed;
            otherCompositeType.Interfaces = Interfaces.Clone();
            otherCompositeType.Events = Events.Clone();
            otherCompositeType.Fields = Fields.Clone();
            otherCompositeType.Constructors = Constructors.Clone();
            otherCompositeType.Indexers = Indexers.Clone();
            otherCompositeType.Properties = Properties.Clone();
            otherCompositeType.Methods = Methods.Clone();
            otherCompositeType.Operators = Operators.Clone();
            otherCompositeType.ConversionOperators = ConversionOperators.Clone();
            otherCompositeType.ExplicitImplementations = ExplicitImplementations.Clone();
        }

        base.CopyDocumentationTo(other);
    }
}