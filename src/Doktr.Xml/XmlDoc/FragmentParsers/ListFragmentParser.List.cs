using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public partial class ListFragmentParser
{
    private const string Bullet = "bullet";
    private const string Numbered = "numbered";

    private static ListFragment ParseList(IXmlDocProcessor processor, XmlNode start, string type)
    {
        bool isValidStyle = GetListStyle(type, out var style);
        if (!isValidStyle)
        {
            var diagnostic = XmlDocDiagnostic.MakeWarning(start.Span, $"Unknown list style '{type}'");
            processor.ReportDiagnostic(diagnostic);
        }

        var list = new ListFragment
        {
            Style = style
        };

        while (processor.Lookahead.IsNotEndElementOrEof())
        {
            var item = ParseListItem(processor);
            list.Items.Add(item);
        }

        return list;
    }

    private static ListItemFragment ParseListItem(IXmlDocProcessor processor)
    {
        var start = processor.ExpectElement(Item);

        // There are 3 scenarios we want to support:
        // 1. <term>Term here</term><description>Description here</description>
        // 2. <description>Description here</description>
        // 3. Description here
        // The first one would be a definition list item,
        // while the second and third one would be a normal/"vanilla" list item.
        ListItemFragment item = processor.Lookahead switch
        {
            XmlElementNode { Name: Term } => ParseDefinitionListItem(processor),
            XmlElementNode { Name: Description } => ParseDescriptionListItem(processor),
            _ => ParseVanillaListItem(processor)
        };

        processor.ExpectEndElement(start.Name);
        return item;
    }

    private static DefinitionListItemFragment ParseDefinitionListItem(IXmlDocProcessor processor)
    {
        var term = ParseTerm(processor);
        var description = ParseDescription(processor);

        return new DefinitionListItemFragment
        {
            Term = term,
            Description = description
        };
    }

    private static VanillaListItemFragment ParseDescriptionListItem(IXmlDocProcessor processor)
    {
        var description = ParseDescription(processor);

        return new VanillaListItemFragment
        {
            Children = description
        };
    }

    private static VanillaListItemFragment ParseVanillaListItem(IXmlDocProcessor processor)
    {
        var fragments = new DocumentationFragmentCollection();
        while (processor.Lookahead.IsNotEndElementOrEof())
            fragments.Add(processor.NextFragment());

        return new VanillaListItemFragment
        {
            Children = fragments
        };
    }

    private static bool GetListStyle(string type, out ListStyle style)
    {
        style = type switch
        {
            Numbered => ListStyle.Numbered,
            _ => ListStyle.Bullet
        };

        return type is Numbered or Bullet;
    }
}