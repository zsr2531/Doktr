using System.Collections.Immutable;
using System.Linq;
using AsmResolver.DotNet;
using Doktr.SemanticDocumentationValidatorTests.TestCases;
using Doktr.Services;
using Moq;
using Serilog;
using Xunit;

namespace Doktr.SemanticDocumentationValidatorTests;

public class NullabilityTest
{
    public class DelegateTest : IClassFixture<TestCaseFixture<Delegate>>
    {
        private readonly TestCaseFixture<Delegate> _fixture;

        public DelegateTest(TestCaseFixture<Delegate> fixture)
        {
            _fixture = fixture;
        }

        public TypeDefinition Type => _fixture.TestType;

        [Fact]
        public void ReturnType()
        {
            var service = new TypeSignatureSourceService();
            using var _ = service.WithContext(Type);
            
            var invoke = Type.Methods.Single(m => m.Name == "Invoke");
            using var __ = service.WithContext(invoke);

            var ret = invoke.Parameters.ReturnParameter;
            string source = service.GetSource(ret.Definition, invoke.Signature.ReturnType, (_, _) => ("", false));
            
            Assert.Equal("object", source);
        }

        [Fact]
        public void Parameters()
        {
            var service = new TypeSignatureSourceService();
            using var _ = service.WithContext(Type);
            
            var invoke = Type.Methods.Single(m => m.Name == "Invoke");
            using var __ = service.WithContext(invoke);

            var parameters = invoke.Parameters;
            var source = parameters.Select(p =>
            {
                var def = p.Definition;
                return service.GetSource(def, p.ParameterType, (_, _) => ("", false));
            });
            
            Assert.Equal(new[] { "object?", "(dynamic, (int, int), string? Heyy)", "int" }, source);
        }
    }
}
