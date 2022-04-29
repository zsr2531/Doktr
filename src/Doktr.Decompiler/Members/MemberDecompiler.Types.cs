using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Decompiler.Members;

public partial class MemberDecompiler
{
    public void VisitClass(ClassDocumentation classDocumentation)
    {
        WriteVisibility(classDocumentation);
        WriteTypeAccessModifiers(classDocumentation);
        _sb.Append("class ");
        _sb.Append(classDocumentation.Name);

        if (!classDocumentation.TypeParameters.IsEmpty())
            WriteTypeParameters(classDocumentation);

        bool hasBaseType = classDocumentation.BaseType is not null;
        bool hasAnyParents = hasBaseType || !classDocumentation.Interfaces.IsEmpty();
        if (!hasAnyParents)
            return;

        WriteParentTypes(hasBaseType
            ? classDocumentation.Interfaces.Prepend(classDocumentation.BaseType!)
            : classDocumentation.Interfaces);
        WriteTypeParameterConstraints(classDocumentation);
    }

    public void VisitInterface(InterfaceDocumentation interfaceDocumentation)
    {
        WriteVisibility(interfaceDocumentation);
        _sb.Append("interface ");
        _sb.Append(interfaceDocumentation.Name);

        if (!interfaceDocumentation.TypeParameters.IsEmpty())
            WriteTypeParameters(interfaceDocumentation);

        bool hasAnyParents = !interfaceDocumentation.Interfaces.IsEmpty();
        if (!hasAnyParents)
            return;

        WriteParentTypes(interfaceDocumentation.Interfaces);
        WriteTypeParameterConstraints(interfaceDocumentation);
    }

    public void VisitRecord(RecordDocumentation recordDocumentation)
    {
        WriteVisibility(recordDocumentation);
        WriteTypeAccessModifiers(recordDocumentation);
        _sb.Append("record ");
        _sb.Append(recordDocumentation.Name);

        if (!recordDocumentation.TypeParameters.IsEmpty())
            WriteTypeParameters(recordDocumentation);

        bool hasBaseType = recordDocumentation.BaseType is not null;
        bool hasAnyParents = hasBaseType || !recordDocumentation.Interfaces.IsEmpty();
        if (!hasAnyParents)
            return;

        WriteParentTypes(hasBaseType
            ? recordDocumentation.Interfaces.Prepend(recordDocumentation.BaseType!)
            : recordDocumentation.Interfaces);
        WriteTypeParameterConstraints(recordDocumentation);
    }

    public void VisitStruct(StructDocumentation structDocumentation)
    {
        WriteVisibility(structDocumentation);
        WriteTypeAccessModifiers(structDocumentation);

        if (structDocumentation.IsReadOnly)
            _sb.Append("readonly ");
        if (structDocumentation.IsByRef)
            _sb.Append("ref ");

        _sb.Append("struct ");
        _sb.Append(structDocumentation.Name);

        if (!structDocumentation.TypeParameters.IsEmpty())
            WriteTypeParameters(structDocumentation);

        bool hasAnyParents = !structDocumentation.Interfaces.IsEmpty();
        if (!hasAnyParents)
            return;

        WriteParentTypes(structDocumentation.Interfaces);
        WriteTypeParameterConstraints(structDocumentation);
    }

    public void VisitDelegate(DelegateDocumentation delegateDocumentation)
    {
        WriteVisibility(delegateDocumentation);
        _sb.Append("delegate ");

        string returnType = DecompileTypeSignature(delegateDocumentation.ReturnType);
        _sb.Append(returnType);
        _sb.Append(' ');

        _sb.Append(delegateDocumentation.Name);
        WriteTypeParameters(delegateDocumentation);
        WriteParameters(delegateDocumentation);
        WriteTypeParameterConstraints(delegateDocumentation);
    }

    private void WriteTypeAccessModifiers(CompositeTypeDocumentation typeDocumentation)
    {
        if (typeDocumentation.IsStatic)
            _sb.Append("static ");
        if (typeDocumentation is not IHasAbstract hasAbstract)
            return;

        if (hasAbstract.IsAbstract)
            _sb.Append("abstract ");
        else if (hasAbstract.IsSealed)
            _sb.Append("sealed ");
    }

    private void WriteParentTypes(IEnumerable<TypeSignature> parentTypes)
    {
        var signatures = parentTypes.Select(DecompileTypeSignature);
        _sb.Append(" : ");
        _sb.AppendJoin(", ", signatures);
    }
}