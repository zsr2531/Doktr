using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public interface IHasCommonTypeCharacteristics
{
    TypeSignatureCollection Interfaces { get; }
    MemberCollection<EventDocumentation> Events { get; }
    MemberCollection<IndexerDocumentation> Indexers { get; }
    MemberCollection<PropertyDocumentation> Properties { get; }
    MemberCollection<MethodDocumentation> Methods { get; }
}