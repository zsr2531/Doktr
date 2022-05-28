namespace Doktr.Core.Models.Constants;

public interface IConstantVisitor
{
    void VisitNull(NullConstant constant);

    void VisitString(StringConstant constant);

    void VisitDefault(DefaultConstant constant);

    void VisitObject(ObjectConstant constant);
}