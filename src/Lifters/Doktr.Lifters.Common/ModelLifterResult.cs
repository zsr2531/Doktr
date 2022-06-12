using Doktr.Core.Models.Collections;

namespace Doktr.Lifters.Common;

public record ModelLifterResult(string AssemblyFullName, TypeDocumentationCollection Types);