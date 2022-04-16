using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public interface IHasTypeParameters : IMemberDocumentation
{
    TypeParameterSegmentCollection TypeParameters { get; set;}
}