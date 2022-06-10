using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc.FragmentParsers;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public class LineBreakFragmentTests : FragmentTests
{
    public LineBreakFragmentTests()
        : base(new LineBreakFragmentParser())
    {
    }

    [Fact]
    public void LineBreak()
    {
        var entry = ParseXmlDoc("<br/>");

        AssertSingleChildIsType<LineBreakFragment>(entry);
    }
}