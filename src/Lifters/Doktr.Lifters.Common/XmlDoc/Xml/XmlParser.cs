using System.Text;
using Antlr4.Runtime;
using Doktr.Lifters.Common.XmlDoc.Collections;

namespace Doktr.Lifters.Common.XmlDoc.Xml;

public class XmlParser : IXmlParser
{
    public XmlNodeCollection ParseXmlNodes(string path)
    {
        var unit = ParseInput(path);
        return CollectNodes(unit);
    }

    private static XmlNodeCollection CollectNodes(Xml.UnitContext unit)
    {
        var nodes = new XmlNodeCollection();
        foreach (var node in unit.node())
            nodes.Add(node.Accept(XmlNodeConstructor.Instance));

        return nodes;
    }

    private static Xml.UnitContext ParseInput(string path)
    {
        var lexer = new XmlLexer(new AntlrFileStream(path, Encoding.UTF8));
        var parser = new Xml(new BufferedTokenStream(lexer));
        var unit = parser.unit();

        return unit;
    }
}