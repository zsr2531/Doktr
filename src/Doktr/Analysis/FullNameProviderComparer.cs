using System.Collections.Generic;
using AsmResolver.DotNet;

namespace Doktr.Analysis
{
    public class FullNameProviderComparer : IEqualityComparer<IFullNameProvider>
    {
        public static readonly FullNameProviderComparer Instance = new();
        
        private FullNameProviderComparer()
        {
        }
        
        public bool Equals(IFullNameProvider x, IFullNameProvider y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.FullName == y.FullName;
        }

        public int GetHashCode(IFullNameProvider obj)
        {
            return obj.FullName != null ? obj.FullName.GetHashCode() : 0;
        }
    }
}