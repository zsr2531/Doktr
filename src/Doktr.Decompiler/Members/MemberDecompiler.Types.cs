using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Decompiler.Members;

public partial class MemberDecompiler
{
    public void VisitClass(ClassDocumentation documentation)
    {
        WriteVisibility(documentation);
        WriteTypeAccessModifiers(documentation);
        _sb.Append("class ");

        WriteParentType(documentation);
        _sb.Append(documentation.Name);

        WriteTypeParameters(documentation);

        bool hasBaseType = documentation.BaseType is not null;
        bool hasAnyParents = hasBaseType || !documentation.Interfaces.IsEmpty();
        if (hasAnyParents)
            WriteBaseTypes(hasBaseType
                ? documentation.Interfaces.Prepend(documentation.BaseType!)
                : documentation.Interfaces);

        WriteTypeParameterConstraintsType(documentation);
    }

    public void VisitInterface(InterfaceDocumentation documentation)
    {
        WriteVisibility(documentation);
        _sb.Append("interface ");

        WriteParentType(documentation);
        _sb.Append(documentation.Name);

        WriteTypeParameters(documentation);

        bool hasAnyParents = !documentation.Interfaces.IsEmpty();
        if (hasAnyParents)
            WriteBaseTypes(documentation.Interfaces);

        WriteTypeParameterConstraintsType(documentation);
    }

    public void VisitRecord(RecordDocumentation documentation)
    {
        WriteVisibility(documentation);
        WriteTypeAccessModifiers(documentation);
        _sb.Append("record ");

        WriteParentType(documentation);
        _sb.Append(documentation.Name);

        WriteTypeParameters(documentation);
        WriteParameters(documentation);

        bool hasBaseType = documentation.BaseType is not null;
        bool hasAnyParents = hasBaseType || !documentation.Interfaces.IsEmpty();
        if (hasAnyParents)
            WriteBaseTypes(hasBaseType
                ? documentation.Interfaces.Prepend(documentation.BaseType!)
                : documentation.Interfaces);

        WriteTypeParameterConstraintsType(documentation);
    }

    public void VisitStruct(StructDocumentation documentation)
    {
        WriteVisibility(documentation);
        WriteTypeAccessModifiers(documentation);

        WriteReadOnly(documentation);
        if (documentation.IsByRef)
            _sb.Append("ref ");

        _sb.Append("struct ");

        WriteParentType(documentation);
        _sb.Append(documentation.Name);

        WriteTypeParameters(documentation);

        bool hasAnyParents = !documentation.Interfaces.IsEmpty();
        if (hasAnyParents)
            WriteBaseTypes(documentation.Interfaces);

        WriteTypeParameterConstraintsType(documentation);
    }

    public void VisitDelegate(DelegateDocumentation documentation)
    {
        WriteVisibility(documentation);
        _sb.Append("delegate ");

        WriteReturnType(documentation);

        _sb.Append(' ');

        WriteParentType(documentation);
        _sb.Append(documentation.Name);

        WriteTypeParameters(documentation);
        WriteParameters(documentation);
        WriteTypeParameterConstraintsType(documentation);
    }

    public void VisitEnum(EnumDocumentation documentation)
    {
        if (documentation.IsFlags)
            _sb.Append("[Flags]\n");

        WriteVisibility(documentation);

        _sb.Append("enum ");

        WriteParentType(documentation);
        _sb.Append(documentation.Name);

        if (documentation.BaseType is not null)
        {
            _sb.Append(" : ");
            WriteTypeSignature(documentation.BaseType);
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