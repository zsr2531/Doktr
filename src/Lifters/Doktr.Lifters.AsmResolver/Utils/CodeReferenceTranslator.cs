using System.Text;
using AsmResolver.DotNet;
using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Lifters.Common.Utils;

namespace Doktr.Lifters.AsmResolver.Utils;

public class CodeReferenceTranslator : ICodeReferenceTranslator<IMemberDefinition>
{
    private const string TypePrefix = "T:";
    private const string EventPrefix = "E:";
    private const string FieldPrefix = "F:";
    private const string PropertyPrefix = "P:";
    private const string MethodPrefix = "M:";
    private const string MethodTypeParameterCountPrefix = "``";
    private const string ImplicitOperatorMethodName = "op_Implicit";
    private const string ExplicitOperatorMethodName = "op_Explicit";
    private const char LeftParenthesis = '(';
    private const char RightParenthesis = ')';
    private const char Tilde = '~';

    private readonly ITypeSignatureTranslator _signatureTranslator;
    private readonly StringBuilder _sb = new();

    public CodeReferenceTranslator(ITypeSignatureTranslator signatureTranslator)
    {
        _signatureTranslator = signatureTranslator;
    }

    public CodeReference TranslateMember(IMemberDefinition member)
    {
        DispatchMember(member);

        string raw = _sb.ToString();
        _sb.Clear();

        return new CodeReference(raw);
    }

    private void DispatchMember(IMemberDefinition member)
    {
        switch (member)
        {
            case TypeDefinition type:
                _sb.Append(TypePrefix);
                TranslateType(type);
                break;

            case EventDefinition ev:
                _sb.Append(EventPrefix);
                TranslateEvent(ev);
                break;

            case FieldDefinition field:
                _sb.Append(FieldPrefix);
                TranslateField(field);
                break;

            case PropertyDefinition property:
                _sb.Append(PropertyPrefix);
                TranslateProperty(property);
                break;

            case MethodDefinition method:
                _sb.Append(MethodPrefix);
                TranslateMethod(method);
                break;
        }
    }

    // Nested types are separated via '+'s in metadata, but in doc id's they are separated via '.'s.
    private void TranslateType(TypeDefinition type) => _sb.Append(type.FullName.Replace('+', '.'));

    private void TranslateEvent(EventDefinition ev) => WriteMemberName(ev);

    private void TranslateField(FieldDefinition field) => WriteMemberName(field);

    private void TranslateProperty(PropertyDefinition property) => WriteMemberName(property);

    private void TranslateMethod(MethodDefinition method)
    {
        WriteMemberName(method);
        WriteTypeParameters();
        WriteParameters();
        WriteReturnType();

        void WriteTypeParameters()
        {
            var typeParameters = method.GenericParameters;
            if (typeParameters.IsEmpty())
                return;

            _sb.Append(MethodTypeParameterCountPrefix);
            _sb.Append(typeParameters.Count);
        }

        void WriteParameters()
        {
            var parameters = method.Parameters;
            if (parameters.IsEmpty())
                return;

            var translatedParameters = parameters.Select(p => p.ParameterType.AcceptVisitor(_signatureTranslator));

            _sb.Append(LeftParenthesis);
            _sb.AppendJoin(',', translatedParameters);
            _sb.Append(RightParenthesis);
        }

        void WriteReturnType()
        {
            if (method.Name != ImplicitOperatorMethodName && method.Name != ExplicitOperatorMethodName)
                return;

            var signature = method.Signature!;
            var returnType = signature.ReturnType;
            string translatedReturnType = returnType.AcceptVisitor(_signatureTranslator);

            _sb.Append(Tilde);
            _sb.Append(translatedReturnType);
        }
    }

    private void WriteMemberName(IMemberDefinition member)
    {
        var type = member.DeclaringType!;
        string name = member.Name!;

        TranslateType(type);
        _sb.Append('.');
        _sb.Append(name.Replace('.', '#'));
    }
}