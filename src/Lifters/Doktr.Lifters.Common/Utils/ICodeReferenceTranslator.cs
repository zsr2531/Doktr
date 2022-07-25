using Doktr.Core.Models;

namespace Doktr.Lifters.Common.Utils;

public interface ICodeReferenceTranslator<T>
    where T : notnull
{
    CodeReference TranslateMember(T member);
}