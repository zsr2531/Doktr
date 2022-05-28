namespace Doktr.Core.Models;

public interface IDocumentationMemberVisitor
{
    void VisitClass(ClassDocumentation documentation);

    void VisitInterface(InterfaceDocumentation documentation);

    void VisitRecord(RecordDocumentation documentation);

    void VisitStruct(StructDocumentation documentation);

    void VisitDelegate(DelegateDocumentation documentation);

    void VisitEnum(EnumDocumentation documentation);

    void VisitEvent(EventDocumentation documentation);

    void VisitField(FieldDocumentation documentation);

    void VisitConstructor(ConstructorDocumentation documentation);

    void VisitFinalizer(FinalizerDocumentation documentation);

    void VisitIndexer(IndexerDocumentation documentation);

    void VisitProperty(PropertyDocumentation documentation);

    void VisitMethod(MethodDocumentation documentation);

    void VisitOperator(OperatorDocumentation documentation);

    void VisitConversionOperator(ConversionOperatorDocumentation documentation);
}