using Doktr.Core.Models;

namespace Doktr.Decompiler.Members;

public partial class MemberDecompiler
{
    public void VisitEvent(EventDocumentation documentation)
    {
        WriteVisibility(documentation);
        WriteStatic(documentation);
        WriteVirtual(documentation);

        _sb.Append("event ");

        WriteTypeSignature(documentation.HandlerType);

        _sb.Append(' ');
        _sb.Append(documentation.Name);
    }

    public void VisitField(FieldDocumentation documentation)
    {
        WriteVisibility(documentation);
        WriteStatic(documentation);
        WriteReadOnly(documentation);
        if (documentation.IsConstant)
            _sb.Append("const ");

        WriteTypeSignature(documentation.Type);

        _sb.Append(' ');
        _sb.Append(documentation.Name);

        if (documentation.IsConstant)
        {
            _sb.Append(" = ");
            documentation.ConstantValue.AcceptVisitor(this);
        }
    }

    public void VisitIndexer(IndexerDocumentation documentation)
    {
        WriteVisibility(documentation);
        WriteStatic(documentation);
        WriteVirtual(documentation);

        WriteTypeSignature(documentation.Type);

        _sb.Append(" this");
        WriteParameters(documentation, '[', ']');

        _sb.Append(" { ");
        if (documentation.Getter is not null)
            WriteGetter(documentation.Visibility, documentation.Getter);
        if (documentation.Setter is not null)
            WriteSetter(documentation.Visibility, documentation.Setter);
        _sb.Append('}');
    }

    public void VisitProperty(PropertyDocumentation documentation)
    {
        WriteVisibility(documentation);
        WriteStatic(documentation);
        WriteVirtual(documentation);

        WriteTypeSignature(documentation.Type);

        _sb.Append(' ');
        _sb.Append(documentation.Name);

        _sb.Append(" { ");
        if (documentation.Getter is not null)
            WriteGetter(documentation.Visibility, documentation.Getter);
        if (documentation.Setter is not null)
            WriteSetter(documentation.Visibility, documentation.Setter);
        _sb.Append('}');
    }

    private void WriteGetter(MemberVisibility parentVisibility, PropertyDocumentation.PropertyGetter getter)
    {
        if (parentVisibility > getter.Visibility)
            WriteVisibility(getter.Visibility);

        _sb.Append("get; ");
    }

    private void WriteSetter(MemberVisibility parentVisibility, PropertyDocumentation.PropertySetter setter)
    {
        if (parentVisibility > setter.Visibility)
            WriteVisibility(setter.Visibility);

        _sb.Append(setter.IsInit
            ? "init; "
            : "set; ");
    }
}