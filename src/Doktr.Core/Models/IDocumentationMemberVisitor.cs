namespace Doktr.Core.Models;

public interface IDocumentationMemberVisitor
{
    void VisitClass(ClassDocumentation classDocumentation);

    void VisitInterface(InterfaceDocumentation interfaceDocumentation);

    void VisitRecord(RecordDocumentation recordDocumentation);

    void VisitStruct(StructDocumentation structDocumentation);

    void VisitDelegate(DelegateDocumentation delegateDocumentation);

    void VisitEnum(EnumDocumentation enumDocumentation);

    void VisitEvent(EventDocumentation eventDocumentation);

    void VisitField(FieldDocumentation fieldDocumentation);

    void VisitConstructor(ConstructorDocumentation constructorDocumentation);

    void VisitFinalizer(FinalizerDocumentation finalizerDocumentation);

    void VisitIndexer(IndexerDocumentation indexerDocumentation);

    void VisitProperty(PropertyDocumentation propertyDocumentation);

    void VisitMethod(MethodDocumentation methodDocumentation);

    void VisitOperator(OperatorDocumentation operatorDocumentation);

    void VisitConversionOperator(ConversionOperatorDocumentation conversionOperatorDocumentation);
}