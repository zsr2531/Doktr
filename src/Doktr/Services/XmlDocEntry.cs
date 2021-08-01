using System.Collections.Immutable;
using Doktr.Models.Segments;

namespace Doktr.Services
{
    public class XmlDocEntry
    {
        public XmlDocEntry(
            ImmutableArray<IDocumentationSegment> summary,
            ImmutableDictionary<string, IDocumentationSegment> parameters,
            ImmutableDictionary<string, IDocumentationSegment> typeParameters,
            ImmutableDictionary<string, IDocumentationSegment> exceptions,
            ImmutableArray<IDocumentationSegment> returns,
            ImmutableArray<IDocumentationSegment> examples,
            ImmutableArray<IDocumentationSegment> remarks,
            ImmutableArray<string> seealso)
        {
            Summary = summary;
            Parameters = parameters;
            TypeParameters = typeParameters;
            Exceptions = exceptions;
            Returns = returns;
            Examples = examples;
            Remarks = remarks;
            Seealso = seealso;
        }

        public ImmutableArray<IDocumentationSegment> Summary
        {
            get;
        }

        public ImmutableDictionary<string, IDocumentationSegment> Parameters
        {
            get;
        }

        public ImmutableDictionary<string, IDocumentationSegment> TypeParameters
        {
            get;
        }

        public ImmutableDictionary<string, IDocumentationSegment> Exceptions
        {
            get;
        }

        public ImmutableArray<IDocumentationSegment> Returns
        {
            get;
        }

        public ImmutableArray<IDocumentationSegment> Examples
        {
            get;
        }

        public ImmutableArray<IDocumentationSegment> Remarks
        {
            get;
        }

        public ImmutableArray<string> Seealso
        {
            get;
        }
    }
}