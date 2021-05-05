using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsmResolver.DotNet.Signatures.Types;

namespace Doktr.Xml.Semantics
{
    public sealed class TypeFormatter : ITypeSignatureVisitor<string>
    {
        public Nullability NullabilityContext
        {
            get;
            set;
        }
        
        public Stack<Nullability> ExplicitNullability
        {
            get;
            set;
        }
        
        public Stack<bool> ExplicitDynamic
        {
            get;
            set;
        }
        
        public Stack<string> TupleElementNames
        {
            get;
            set;
        }
        
        public string VisitArrayType(ArrayTypeSignature signature)
        {
            string type = signature.BaseType.AcceptVisitor(this);
            string arrayDimensions = new(',', signature.Dimensions.Count);
            string nullability = GetNullability();

            return $"{type}[{arrayDimensions}]{nullability}";
        }

        public string VisitBoxedType(BoxedTypeSignature signature)
        {
            throw new NotSupportedException();
        }

        public string VisitByReferenceType(ByReferenceTypeSignature signature)
        {
            return signature.BaseType.AcceptVisitor(this);
        }

        public string VisitCorLibType(CorLibTypeSignature signature)
        {
            if (signature.IsValueType)
                return signature.Name;

            string nullability = GetNullability();

            return $"{signature.Name}{nullability}";
        }

        public string VisitCustomModifierType(CustomModifierTypeSignature signature)
        {
            return signature.BaseType.AcceptVisitor(this);
        }

        public string VisitGenericInstanceType(GenericInstanceTypeSignature signature)
        {
            string type = signature.GenericType.Name;
            string nullability = signature.IsValueType ? "" : GetNullability();
            var sb = new StringBuilder(type);
            sb.Append('<');

            var genericParameters = signature.TypeArguments.Select(s => s.AcceptVisitor(this));
            sb.Append(string.Join(", ", genericParameters));

            sb.Append('>');

            if (signature.IsValueType)
                return sb.ToString();

            sb.Append(nullability);

            return sb.ToString();
        }

        public string VisitGenericParameter(GenericParameterSignature signature)
        {
            if (signature.IsValueType)
                return signature.Name;

            string nullability = GetNullability();
            return $"{signature.Name}{nullability}";
        }

        public string VisitPinnedType(PinnedTypeSignature signature)
        {
            throw new NotSupportedException();
        }

        public string VisitPointerType(PointerTypeSignature signature)
        {
            string @base = signature.BaseType.AcceptVisitor(this);

            return $"{@base}*";
        }

        public string VisitSentinelType(SentinelTypeSignature signature)
        {
            throw new NotSupportedException();
        }

        public string VisitSzArrayType(SzArrayTypeSignature signature)
        {
            string type = signature.BaseType.AcceptVisitor(this);
            string nullability = GetNullability();

            return $"{type}[]{nullability}";
        }

        public string VisitTypeDefOrRef(TypeDefOrRefSignature signature)
        {
            string name = signature.Name;
            string nullability = GetNullability();
            int tick = name.IndexOf('`');
            string noTicks = tick < 0
                ? name
                : name[..^tick];

            return $"{noTicks}{nullability}";
        }

        private string GetNullability()
        {
            var nullability = ExplicitNullability.Count == 0
                ? NullabilityContext
                : ExplicitNullability.Pop();

            return nullability.ToString();
        }
    }
}