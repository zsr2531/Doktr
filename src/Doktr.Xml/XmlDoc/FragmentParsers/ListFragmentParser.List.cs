using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public partial class ListFragmentParser
{
    private const string Bullet = "bullet";
    private const string Numbered = "numbered";

    private static ListFragment ParseList(IXmlDocProcessor processor, string type)
    {
        var style = GetListStyle(type);
        var list = new ListFragment
        {
            Style = style
        };

        while (processor.Lookahead.IsNotEndElementOrNull())
        {
            var item = ParseListItem(processor);
            list.Items.Add(item);
        }

        return list;
    }

    private static ListItemFragment ParseListItem(IXmlDocProcessor processor)
    {
        // This will allocate a new array for every call...
        // Maybe this can be tweaked in the future?
        // I just wish Roslyn would outline this to a
        // static readonly field, so it wouldn't allocate every
        // single time... But one can only wish.
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
        while (processor.Lookahead.IsNotEndElementOrNull())
            fragments.Add(processor.NextFragment());

        return new VanillaListItemFragment
        {
            Children = fragments
        };
    }

    private static ListStyle GetListStyle(string type)
    {
        return type switch
        {
            Bullet => ListStyle.Bullet,
            Numbered => ListStyle.Numbered,
            _ => ListStyle.Bullet // TODO: Warn here for invalid list style.
        };
    }
}