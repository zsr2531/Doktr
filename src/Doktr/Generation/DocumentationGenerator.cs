using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using AsmResolver.DotNet;
using Doktr.Xml;

namespace Doktr.Generation
{
    public class DocumentationGenerator
    {
        private readonly IDocumentationVisitor _visitor;
        private readonly IDictionary<IFullNameProvider, ImmutableArray<IXmlDocSegment>> _documentation;

        public DocumentationGenerator(
            IDocumentationVisitor visitor,
            IDictionary<IFullNameProvider, ImmutableArray<IXmlDocSegment>> documentation)
        {
            _visitor = visitor;
            _documentation = documentation;
        }

        public void GenerateDocumentation(IFullNameProvider member)
        {
            if (!_documentation.ContainsKey(member))
                throw new InvalidOperationException();

            var segments = _documentation[member];
            foreach (var segment in segments)
                segment.AcceptVisitor(_visitor);
        }
    }
}