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
        
        public IReference ReturnType
        {
            get;
        }
        
        public ImmutableArray<ParameterDocumentation> Parameters
        {
            get;
        }
    }
}