using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc.FragmentParsers;
using FluentAssertions;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public class TypeParameterReferenceFragmentTests : FragmentTests
{
    public TypeParameterReferenceFragmentTests()
        : base(new TypeParameterReferenceFragmentParser())
    {
    }

    [Fact]
    public void TypeParameterReference()
    {
        var entry = GetSummaryFor("<typeparamref name='test'/>");

        var typeparamref = AssertSingleChildIsType<TypeParameterReferenceFragment>(entry);
        typeparamref.Name.Should().Be("test");
    }

    [Fact]
    public void Missing_Name()
    {
        var parser = CreateParser("<typeparamref/>");
        var result = parser.ParseXmlDoc();

        Assert.NotEmpty(result);
        Assert.True(parser.HasErrors);
        Assert.False(parser.HasFatalErrors);
    }
}