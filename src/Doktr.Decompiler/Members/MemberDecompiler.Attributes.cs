using Doktr.Core.Models;

namespace Doktr.Decompiler.Members;

public partial class MemberDecompiler
{
    public void VisitEvent(EventDocumentation eventDocumentation)
    {
        WriteVisibility(eventDocumentation);
        WriteStatic(eventDocumentation);
        WriteVirtual(eventDocumentation);

        _sb.Append("event ");

        WriteTypeSignature(eventDocumentation.HandlerType);

        _sb.Append(' ');
        _sb.Append(eventDocumentation.Name);
    }

    public void VisitField(FieldDocumentation fieldDocumentation)
    {
        WriteVisibility(fieldDocumentation);
        WriteStatic(fieldDocumentation);
        WriteReadOnly(fieldDocumentation);
        if (fieldDocumentation.IsConstant)
            _sb.Append("const ");

        WriteTypeSignature(fieldDocumentation.Type);

        _sb.Append(' ');
        _sb.Append(fieldDocumentation.Name);

        if (fieldDocumentation.IsConstant)
        {
            _sb.Append(" = ");
            fieldDocumentation.ConstantValue.AcceptVisitor(this);
        }
    }

    public void VisitIndexer(IndexerDocumentation indexerDocumentation)
    {
        WriteVisibility(indexerDocumentation);
        WriteStatic(indexerDocumentation);
        WriteVirtual(indexerDocumentation);

        WriteTypeSignature(indexerDocumentation.Type);

        _sb.Append(" this");
        WriteParameters(indexerDocumentation, '[', ']');

        _sb.Append(" { ");
        if (indexerDocumentation.Getter is not null)
            WriteGetter(indexerDocumentation.Visibility, indexerDocumentation.Getter);
        if (indexerDocumentation.Setter is not null)
            WriteSetter(indexerDocumentation.Visibility, indexerDocumentation.Setter);
        _sb.Append('}');
    }

    public void VisitProperty(PropertyDocumentation propertyDocumentation)
    {
        WriteVisibility(propertyDocumentation);
        WriteStatic(propertyDocumentation);
        WriteVirtual(propertyDocumentation);

        WriteTypeSignature(propertyDocumentation.Type);

        _sb.Append(' ');
        _sb.Append(propertyDocumentation.Name);

        _sb.Append(" { ");
        if (propertyDocumentation.Getter is not null)
            WriteGetter(propertyDocumentation.Visibility, propertyDocumentation.Getter);
        if (propertyDocumentation.Setter is not null)
            WriteSetter(propertyDocumentation.Visibility, propertyDocumentation.Setter);
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