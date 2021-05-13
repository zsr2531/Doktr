using System.Collections.Immutable;
using Doktr.Models.References;

namespace Doktr.Models
{
    public class DelegateDocumentation : TypeDocumentation
    {
        public DelegateDocumentation(
            string assembly,
            string ns,
            string name,
            IReference returnType,
            ImmutableArray<ParameterDocumentation> parameters)
            : base(assembly, ns, name)
        {
            ReturnType = returnType;
            Parameters = parameters;
        }

        public DelegateDocumentation(DelegateDocumentation other)
            : base(other.Assembly, other.Namespace, other.Name)
        {
            ReturnType = other.ReturnType;
            Parameters = other.Parameters;
        }
        
        public IReference ReturnType
        {
            get;
            init;
        }
        
        public ImmutableArray<ParameterDocumentation> Parameters
        {
            get;
            init;
        }
    }
}