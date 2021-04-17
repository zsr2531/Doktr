using System;
using System.Linq;
using AsmResolver.DotNet.Signatures.Types;

namespace Doktr.Resolution
{
    public class TypeSignatureVisitor : ITypeSignatureVisitor<string>
    {
        private readonly bool _dropBackticks;

        public TypeSignatureVisitor(bool dropBackticks)
        {
            _dropBackticks = dropBackticks;
        }

        public string VisitArrayType(ArrayTypeSignature signature)
        {
            var type = signature.BaseType;
            int dimensions = signature.Dimensions.Count;

            return $"{type.AcceptVisitor(this)}[{string.Join(",", Enumerable.Repeat("0:", dimensions))}]";
        }

        public string VisitBoxedType(BoxedTypeSignature signature)
        {
            throw new NotImplementedException();
        }

        public string VisitByReferenceType(ByReferenceTypeSignature signature)
        {
            var type = signature.BaseType;

            return $"{type.AcceptVisitor(this)}@";
        }

        public string VisitCorLibType(CorLibTypeSignature signature)
        {
            return signature.Type.FullName;
        }

        public string VisitCustomModifierType(CustomModifierTypeSignature signature)
        {
            var type = signature.BaseType;

            return type.AcceptVisitor(this);
        }

        public string VisitGenericInstanceType(GenericInstanceTypeSignature signature)
        {
            string name = signature.GenericType.FullName;
            int indexOfBackTick = name.IndexOf('`');
            string realName = name[..indexOfBackTick];
            string parameters = string.Join("", signature.TypeArguments.Select(a => a.AcceptVisitor(this))).Replace('<', '{').Replace('>', '}').Replace('!', '`');

            return realName + '{' + parameters + '}';
        }

        public string VisitGenericParameter(GenericParameterSignature signature)
        {
            return signature.Name.Replace('!', '`');
        }

        public string VisitPinnedType(PinnedTypeSignature signature)
        {
            throw new NotImplementedException();
        }

        public string VisitPointerType(PointerTypeSignature signature)
        {
            var type = signature.BaseType;

            return $"{type.AcceptVisitor(this)}*";
        }

        public string VisitSentinelType(SentinelTypeSignature signature)
        {
            throw new NotImplementedException();
        }

        public string VisitSzArrayType(SzArrayTypeSignature signature)
        {
            var type = signature.BaseType;

            return $"{type.AcceptVisitor(this)}[]";
        }

        public string VisitTypeDefOrRef(TypeDefOrRefSignature signature)
        {
            return signature.Type.FullName;
        }
    }
}