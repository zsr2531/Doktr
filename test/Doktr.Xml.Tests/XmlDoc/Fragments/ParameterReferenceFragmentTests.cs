using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc.FragmentParsers;
using FluentAssertions;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public class ParameterReferenceFragmentTests : FragmentTests
{
    public ParameterReferenceFragmentTests()
        : base(new ParameterReferenceFragmentParser())
    {
    }

    [Fact]
    public void ParameterReference()
    {
        var entry = ParseXmlDoc("<paramref name='test'/>");

        var paramref = AssertSingleChildIsType<ParameterReferenceFragment>(entry);
        paramref.Name.Should().Be("test");
    }

    [Fact]
    public void Missing_Name()
    {
        var parser = CreateParser("<paramref/>");
        var result = parser.ParseXmlDoc();

        Assert.Single(result);
        Assert.NotEmpty(parser.Diagnostics);
        Assert.False(parser.HasFatalErrors);
    }
}