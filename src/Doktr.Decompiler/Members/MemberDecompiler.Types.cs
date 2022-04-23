using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Decompiler.Members;

internal partial class MemberDecompiler
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

        _sb.Append(" : ");
        WriteParentTypes(hasBaseType
            ? classDocumentation.Interfaces.Prepend(classDocumentation.BaseType!)
            : classDocumentation.Interfaces);
    }

    public void VisitInterface(InterfaceDocumentation interfaceDocumentation) => throw new NotImplementedException();

    public void VisitRecord(RecordDocumentation recordDocumentation) => throw new NotImplementedException();

    public void VisitStruct(StructDocumentation structDocumentation) => throw new NotImplementedException();

    public void VisitDelegate(DelegateDocumentation delegateDocumentation) => throw new NotImplementedException();

    private void WriteTypeAccessModifiers(CompositeTypeDocumentation typeDocumentation)
    {
        if (typeDocumentation.IsStatic)
            _sb.Append("static ");
        else if (typeDocumentation.IsAbstract)
            _sb.Append("abstract ");
        else if (typeDocumentation.IsSealed)
            _sb.Append("sealed ");
    }

    private void WriteParentTypes(IEnumerable<TypeSignature> parentTypes)
    {
        var signatures = parentTypes.ToArray();
        for (int i = 0; i < signatures.Length; i++)
        {
            var current = signatures[i];
            string decompiled = DecompileTypeSignature(current);
            _sb.Append(decompiled);

            if (i + 1 < signatures.Length)
                _sb.Append(", ");
        }
    }
}