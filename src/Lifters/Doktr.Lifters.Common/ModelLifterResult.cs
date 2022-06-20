using System.Diagnostics.CodeAnalysis;
using Doktr.Core.Models.Collections;

namespace Doktr.Lifters.Common;

[ExcludeFromCodeCoverage]
public record ModelLifterResult(string AssemblyFullName, TypeDocumentationCollection Types);