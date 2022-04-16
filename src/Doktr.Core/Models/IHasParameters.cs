using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public interface IHasParameters : IMemberDocumentation
{
    ParameterSegmentCollection Parameters { get; set; }
}