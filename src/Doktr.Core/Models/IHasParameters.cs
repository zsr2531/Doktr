using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public interface IHasParameters
{
    ParameterSegmentCollection Parameters { get; set; }
}