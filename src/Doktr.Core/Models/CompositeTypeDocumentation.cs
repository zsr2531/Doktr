using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public abstract class CompositeTypeDocumentation : TypeDocumentation
{
    protected CompositeTypeDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public TypeSignatureCollection Interfaces { get; set; } = new();
    public MemberCollection<EventDocumentation> Events { get; set; } = new();
    public MemberCollection<FieldDocumentation> Fields { get; set; } = new();
    public MemberCollection<ConstructorDocumentation> Constructors { get; set; } = new();
    public FinalizerDocumentation? Finalizer { get; set; }
    public MemberCollection<IndexerDocumentation> Indexers { get; set; } = new();
    public MemberCollection<PropertyDocumentation> Properties { get; set; } = new();
    public MemberCollection<MethodDocumentation> Methods { get; set; } = new();
    public MemberCollection<OperatorDocumentation> Operators { get; set; } = new();
    public MemberCollection<ConversionOperatorDocumentation> ConversionOperators { get; set; } = new();
    public ExplicitImplementationCollection ExplicitImplementations { get; set; } = new();

    public abstract override CompositeTypeDocumentation Clone();

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is not CompositeTypeDocumentation otherCompositeType)
            throw new InvalidOperationException("Cannot copy documentation to non-composite type member.");

        otherCompositeType.Interfaces = Interfaces.Clone();
        otherCompositeType.Events = Events.Clone();
        otherCompositeType.Fields = Fields.Clone();
        otherCompositeType.Constructors = Constructors.Clone();
        otherCompositeType.Finalizer = Finalizer?.Clone();
        otherCompositeType.Indexers = Indexers.Clone();
        otherCompositeType.Properties = Properties.Clone();
        otherCompositeType.Methods = Methods.Clone();
        otherCompositeType.Operators = Operators.Clone();
        otherCompositeType.ConversionOperators = ConversionOperators.Clone();
        otherCompositeType.ExplicitImplementations = ExplicitImplementations.Clone();
        base.CopyDocumentationTo(other);
    }
}