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

        string handlerType = DecompileTypeSignature(eventDocumentation.HandlerType);
        _sb.Append(handlerType);

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

        string type = DecompileTypeSignature(fieldDocumentation.Type);
        _sb.Append(type);

        _sb.Append(' ');
        _sb.Append(fieldDocumentation.Name);

        if (fieldDocumentation.IsConstant)
        {
            _sb.Append(" = ");
            _sb.Append(fieldDocumentation.ConstantValue ?? "null");
        }
    }

    public void VisitIndexer(IndexerDocumentation indexerDocumentation)
    {
        WriteVisibility(indexerDocumentation);
        WriteStatic(indexerDocumentation);
        WriteVirtual(indexerDocumentation);

        string type = DecompileTypeSignature(indexerDocumentation.Type);
        _sb.Append(type);

        _sb.Append(" this");
        WriteParameters(indexerDocumentation, '[', ']');

        _sb.Append(" { ");
        if (indexerDocumentation.Getter is not null)
            _sb.Append("get; ");
        if (indexerDocumentation.Setter is { IsInit: false })
            _sb.Append("set; ");
        if (indexerDocumentation.Setter is { IsInit: true })
            _sb.Append("init; ");
        _sb.Append('}');
    }

    public void VisitProperty(PropertyDocumentation propertyDocumentation)
    {
        WriteVisibility(propertyDocumentation);
        WriteStatic(propertyDocumentation);
        WriteVirtual(propertyDocumentation);

        string type = DecompileTypeSignature(propertyDocumentation.Type);
        _sb.Append(type);

        _sb.Append(' ');
        _sb.Append(propertyDocumentation.Name);

        _sb.Append(" { ");
        if (propertyDocumentation.Getter is not null)
            _sb.Append("get; ");
        if (propertyDocumentation.Setter is { IsInit: false })
            _sb.Append("set; ");
        if (propertyDocumentation.Setter is { IsInit: true })
            _sb.Append("init; ");
        _sb.Append('}');
    }
}