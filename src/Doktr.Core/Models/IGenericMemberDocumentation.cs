using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public interface IGenericMemberDocumentation : IMemberDocumentation
{
    TypeParameterSegmentCollection TypeParameters { get; set;}
}