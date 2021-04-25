using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;

namespace Doktr.Analysis.Transformations
{
    public class ConstructorTransformer : DependencyAdderTransformerBase
    {
        protected override void Visit(DependencyNode node, TransformationContext context)
        {
            if (node.MetadataMember is not MethodDefinition { IsConstructor: true, CilMethodBody: {} body })
                return;

            foreach (var instruction in body.Instructions)
            {
                if (instruction.OpCode.Code == CilCode.Call && instruction.Operand is IMethodDefOrRef ctor)
                {
                    var resolved = ctor.Resolve();
                    if (resolved?.IsConstructor ?? false)
                    {
                        Depend(node, context.GetOrCreateNode(resolved));
                        break;
                    }
                }
            }
        }
    }
}