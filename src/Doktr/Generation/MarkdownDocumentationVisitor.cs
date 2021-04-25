using System.IO;
using Doktr.Xml;

namespace Doktr.Generation
{
    public class MarkdownDocumentationVisitor : IDocumentationVisitor
    {
        private readonly TextWriter _writer;

        public MarkdownDocumentationVisitor(TextWriter writer)
        {
            _writer = writer;
        }

        public void Visit(MonospaceXmlDocSegment segment)
        {
            _writer.WriteLine("```csharp");
            
            foreach (var nested in segment.Content)
                nested.AcceptVisitor(this);
            
            _writer.WriteLine("```");
        }

        public void Visit(DescriptionXmlDocSegment segment)
        {
            foreach (var nested in segment.Content)
                nested.AcceptVisitor(this);
        }

        public void Visit(ExceptionXmlDocSegment segment)
        {
            _writer.Write($"`{segment.Cref}`: ");

            foreach (var nested in segment.Content)
                nested.AcceptVisitor(this);

            _writer.WriteLine();
        }

        public void Visit(InheritDocXmlDocSegment segment)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ItemXmlDocSegment segment)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ListXmlDocSegment segment)
        {
            // TODO: Impl
            //throw new System.NotImplementedException();
        }

        public void Visit(ParamrefXmlDocSegment segment)
        {
            _writer.Write($"*{segment.Parameter}*");
        }

        public void Visit(ParamXmlDocSegment segment)
        {
            _writer.Write($"`{segment.Parameter}`: ");
            
            foreach (var nested in segment.Content)
                nested.AcceptVisitor(this);

            _writer.WriteLine();
        }

        public void Visit(ParaXmlDocSegment segment)
        {
            foreach (var nested in segment.Content)
                nested.AcceptVisitor(this);
        }

        public void Visit(RawXmlDocSegment segment)
        {
            _writer.Write(segment.Content);
        }

        public void Visit(RemarksXmlDocSegment segment)
        {
            _writer.Write("Remarks: ");
            
            foreach (var nested in segment.Content)
                nested.AcceptVisitor(this);
            
            _writer.WriteLine();
        }

        public void Visit(ReturnsXmlDocSegment segment)
        {
            _writer.Write("Returns: ");
            
            foreach (var nested in segment.Content)
                nested.AcceptVisitor(this);
            
            _writer.WriteLine();
        }

        public void Visit(SeealsoXmlDocSegment segment)
        {
            _writer.Write($"{segment.Cref}");
        }

        public void Visit(SeeXmlDocSegment segment)
        {
            _writer.Write($"{segment.Cref}");
        }

        public void Visit(StrongXmlDocSegment segment)
        {
            _writer.Write("**");
            
            foreach (var nested in segment.Content)
                nested.AcceptVisitor(this);
            
            _writer.Write("**");
        }

        public void Visit(SummaryXmlDocSegment segment)
        {
            foreach (var nested in segment.Content)
                nested.AcceptVisitor(this);
            
            _writer.WriteLine();
        }

        public void Visit(TypeParamrefXmlDocSegment segment)
        {
            _writer.Write($"***{segment.TypeParameter}***");
        }

        public void Visit(TypeParamXmlDocSegment segment)
        {
            _writer.Write($"`{segment.TypeParameter}`: ");
            
            foreach (var nested in segment.Content)
                nested.AcceptVisitor(this);

            _writer.WriteLine();
        }
    }
}