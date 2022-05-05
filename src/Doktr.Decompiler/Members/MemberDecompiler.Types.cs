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

        WriteParentType(classDocumentation);
        _sb.Append(classDocumentation.Name);

        WriteTypeParameters(classDocumentation);

        bool hasBaseType = classDocumentation.BaseType is not null;
        bool hasAnyParents = hasBaseType || !classDocumentation.Interfaces.IsEmpty();
        if (hasAnyParents)
            WriteBaseTypes(hasBaseType
                ? classDocumentation.Interfaces.Prepend(classDocumentation.BaseType!)
                : classDocumentation.Interfaces);

        WriteTypeParameterConstraintsType(classDocumentation);
    }

    public void VisitInterface(InterfaceDocumentation interfaceDocumentation)
    {
        WriteVisibility(interfaceDocumentation);
        _sb.Append("interface ");
        _sb.Append(interfaceDocumentation.Name);

        WriteTypeParameters(interfaceDocumentation);

        bool hasAnyParents = !interfaceDocumentation.Interfaces.IsEmpty();
        if (hasAnyParents)
            WriteBaseTypes(interfaceDocumentation.Interfaces);

        WriteTypeParameterConstraintsType(interfaceDocumentation);
    }

    public void VisitRecord(RecordDocumentation recordDocumentation)
    {
        WriteVisibility(recordDocumentation);
        WriteTypeAccessModifiers(recordDocumentation);
        _sb.Append("record ");
        _sb.Append(recordDocumentation.Name);

        WriteTypeParameters(recordDocumentation);
        WriteParameters(recordDocumentation);

        bool hasBaseType = recordDocumentation.BaseType is not null;
        bool hasAnyParents = hasBaseType || !recordDocumentation.Interfaces.IsEmpty();
        if (hasAnyParents)
            WriteBaseTypes(hasBaseType
                ? recordDocumentation.Interfaces.Prepend(recordDocumentation.BaseType!)
                : recordDocumentation.Interfaces);

        WriteTypeParameterConstraintsType(recordDocumentation);
    }

    public void VisitStruct(StructDocumentation structDocumentation)
    {
        WriteVisibility(structDocumentation);
        WriteTypeAccessModifiers(structDocumentation);

        WriteReadOnly(structDocumentation);
        if (structDocumentation.IsByRef)
            _sb.Append("ref ");

        _sb.Append("struct ");
        _sb.Append(structDocumentation.Name);

        WriteTypeParameters(structDocumentation);

        bool hasAnyParents = !structDocumentation.Interfaces.IsEmpty();
        if (hasAnyParents)
            WriteBaseTypes(structDocumentation.Interfaces);

        WriteTypeParameterConstraintsType(structDocumentation);
    }

    public void VisitDelegate(DelegateDocumentation delegateDocumentation)
    {
        WriteVisibility(delegateDocumentation);
        _sb.Append("delegate ");

        WriteReturnType(delegateDocumentation);

        _sb.Append(' ');
        _sb.Append(delegateDocumentation.Name);

        WriteTypeParameters(delegateDocumentation);
        WriteParameters(delegateDocumentation);
        WriteTypeParameterConstraintsType(delegateDocumentation);
    }

    public void VisitEnum(EnumDocumentation enumDocumentation)
    {
        if (enumDocumentation.IsFlags)
            _sb.Append("[Flags]\n");

        WriteVisibility(enumDocumentation);

        _sb.Append("enum ");
        _sb.Append(enumDocumentation.Name);

        if (enumDocumentation.BaseType is not null)
        {
            _sb.Append(" : ");
            WriteTypeSignature(enumDocumentation.BaseType);
        }
    }

    private void WriteTypeAccessModifiers(CompositeTypeDocumentation typeDocumentation)
    {
        WriteStatic(typeDocumentation);
        if (typeDocumentation is not IHasAbstract hasAbstract)
            return;

        if (hasAbstract.IsAbstract)
            _sb.Append("abstract ");
        else if (hasAbstract.IsSealed)
            _sb.Append("sealed ");
    }

    private void WriteBaseTypes(IEnumerable<TypeSignature> parentTypes)
    {
        var signatures = parentTypes.ToArray();
        _sb.Append(" : ");

        for (int i = 0; i < signatures.Length; i++)
        {
            var current = signatures[i];
            WriteTypeSignature(current);

            if (i + 1 < signatures.Length)
                _sb.Append(", ");
        }
    }

    private void WriteParentType(TypeDocumentation type)
    {
        var parent = type.ParentType;
        if (parent is null)
            return;

        WriteParentType(parent);

        _sb.Append(parent.Name);
        WriteTypeParameters(parent);

        _sb.Append('.');
    }

    private void WriteTypeParameterConstraintsType(TypeDocumentation type)
    {
        var parent = type.ParentType;
        if (parent is not null)
            WriteTypeParameterConstraintsType(parent);

        if (type is not IHasTypeParameters withTypeParams)
            return;

        WriteTypeParameterConstraints(withTypeParams);
    }
}