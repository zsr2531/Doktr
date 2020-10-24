using System.Diagnostics;
using AsmResolver.DotNet;

namespace Doktr.Analysis
{
    [DebuggerDisplay("{" + nameof(MetadataMember) + ",nq}")]
    public class ExternalDependencyNode : DependencyNodeBase
    {
        public ExternalDependencyNode(INameProvider metadataMember)
            : base(metadataMember) { }
    }
}